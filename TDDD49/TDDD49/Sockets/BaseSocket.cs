using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.IO;
using System.Threading.Tasks;
using TDDD49.Models;
using TDDD49.Models.Messages;
using TDDD49.Utility;
using TDDD49.Exceptions;

namespace TDDD49.Sockets
{
    public abstract class BaseSocket
    {
        public delegate void OnMessageRecievedEvent(Message message, MessageType type);

        public delegate void OnDisconnectEvent();


        public event OnMessageRecievedEvent OnMessageRecieved;

        public event OnDisconnectEvent OnSocketDisconnect;


        public delegate void MessageParseExceptionEvent(MessageParseException ex);
        public event MessageParseExceptionEvent OnMessageParseException;


        protected const int HEADER_SIZE = 512;
        protected const int REQUEST_SIZE = 1024;
        protected NetworkStream networkStream;
        protected TcpClient client;

        private bool isConnected = true;


        public void ClosePeerConnection()
        {
            isConnected = false;
            networkStream?.Dispose();
            networkStream?.Close();
            client?.Dispose();
            client?.Close();
            thread.Abort();

        }

        protected void WriteFull(byte[] bytes)
        {
            try
            {
                lock (networkStream)
                {
                    networkStream.Write(bytes, 0, bytes.Length);
                    networkStream.Flush();
                }
            }
            catch(Exception ex)
            {               
                Console.WriteLine(ex.ToString());
                //Disconnect();
                throw new PeerDisconnectException("You peer disconnected");
            }
        }

        public void SendAnswer(RequestMessage requestMessage)
        {
            string msg = MessageParser.ToJson(requestMessage);

            byte[] byteArrayToSend = Encoding.Default.GetBytes(msg);
            WriteFull(byteArrayToSend);
        }



        public void SendMessage(Message message, MessageType type)
        {
            string msg = MessageParser.ToJson(message); 
            int contentSize = msg.Length;
            Header header = new Header(contentSize, type);
            string headermsg = MessageParser.ToJson(header); 
            byte[] headerByteArray = Encoding.Default.GetBytes(headermsg);
            byte[] contentArrayToSend = Encoding.Default.GetBytes(msg);

            int wholeSize = HEADER_SIZE + contentSize;

            byte[] wholeByteArray = new byte[wholeSize];
            Array.Copy(headerByteArray, 0, wholeByteArray, 0, headerByteArray.Length);
            Array.Copy(contentArrayToSend, 0, wholeByteArray, HEADER_SIZE, contentSize); // TODO + 1


            WriteFull(wholeByteArray);
        }

        private void Disconnect()
        {
            isConnected = false;
            client.Dispose();
            client.Close();
            OnSocketDisconnect?.Invoke();
        }

        private async Task PollLoop() 
        {
            Header poll = new Header(0, MessageType.PollMessage);
            while (isConnected)
            {             
                try
                {
                    WriteFull(Encoding.Default.GetBytes(MessageParser.ToJson(poll)));
                }
                catch(PeerDisconnectException ex)
                {
                    Console.WriteLine(ex.ToString());
                    isConnected = false;
                    // Disconnect();
                    return;
                }
                await Task.Delay(4000);
            }         
        }
      
        private void ReadMessages()
        {
            client.ReceiveBufferSize = Int32.MaxValue;
            Console.WriteLine("started reading mesgeges");
            while (isConnected)
            {
                try
                {
                    if (!networkStream.DataAvailable) { continue; }

                    byte[] byteForm = new byte[HEADER_SIZE];
                    networkStream.Read(byteForm, 0, HEADER_SIZE);

                    string headerMsg = Encoding.Default.GetString(byteForm, 0, HEADER_SIZE).Trim('\0');
                    Header header = JsonConvert.DeserializeObject<Header>(headerMsg);
                   
                    if (header.messageType == MessageType.PollMessage)                   
                        continue;
                    
                    int amount = header.size;
                    string typeName = header.messageType.ToString();
                    byte[] contentByteForm = new byte[amount];

                    int bytesRead = 0;
                    string totalData = "";
                    int chunkSize = amount;

                    while(true)
                    {
                        byte[] imageChunk = new byte[chunkSize];
                        networkStream.Read(imageChunk, 0, chunkSize);
                        string chunkStr = Encoding.Default.GetString(imageChunk, 0, chunkSize).Trim('\0');
                        totalData += chunkStr;
                        bytesRead += chunkStr.Length;

                        if(totalData.Length < amount)
                        {
                            chunkSize = amount - totalData.Length;
                        }
                        else
                        {
                            break;
                        }
                    }

                    string messageContentString = totalData;
                    Message msg = MessageParser.FromJson<Message>(messageContentString);
                    Runner.Primary(() => OnMessageRecieved?.Invoke(msg, header.messageType));
                }              

                catch(MessageParseException ex)
                {
                    Console.WriteLine(ex.ToString());
                    //   throw new CorruptDataException("Data corrupt"); //invoke
                  
                    OnMessageParseException?.Invoke(ex);
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());

                    break;
                }
                finally
                {
                    networkStream.Flush();
                    
                }
                
            }
           Disconnect();
        }

        public Thread thread;
        public void StartReadingMessages()
        {

            thread = Runner.Secondary(() =>
            {
                Task.Run(() => PollLoop());
                ReadMessages();
            });
           
        }
    }
}
