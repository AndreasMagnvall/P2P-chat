using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TDDD49.Models;
using TDDD49.Models.Messages;
using TDDD49.Utility;

namespace TDDD49.Sockets
{
    public class SyncClientSocket : BaseSocket
    {
        public string Ip { get; set; }
        public int RequestedPort { get; set; }

        public delegate void OnClientConnectEvent();
        public delegate void OnPeerAnswerFromServerEvent(RequestMessage requestMessage);

        public event OnClientConnectEvent onClientConnection;
        public event OnPeerAnswerFromServerEvent OnPeerAccepted;
        public event OnPeerAnswerFromServerEvent OnPeerRejected;
        

        protected void OnClientConnection()
        {
            OnClientConnectEvent handler = onClientConnection;
            handler?.Invoke();
        }
        private void OnServerPeerAnswer(RequestMessage requestMessage) 
        {
            OnPeerAnswerFromServerEvent peerEvent;
            //Console.WriteLine(answer);
            peerEvent = requestMessage.answer ? OnPeerAccepted : OnPeerRejected;
            Console.WriteLine("invoking event");
            Console.WriteLine(peerEvent.ToString());
            peerEvent?.Invoke(requestMessage);
        }
      
        public SyncClientSocket() { }
     
        public void ListenForRequestAnswer()
        {
            byte[] bytesFrom = new byte[1024];

            networkStream.Read(bytesFrom, 0, 1024);
            networkStream.Flush();

            //trim removes not set parts of bytesfrom
            string msg = Encoding.Default.GetString(bytesFrom, 0, 1024).Trim('\0');

            Console.WriteLine(msg);

            RequestMessage message = MessageParser.FromJson<RequestMessage>(msg);
            Console.WriteLine("!!!###### ListenForRequestAnswer got answer !!!!!!!########" + message.answer);
            OnServerPeerAnswer(message);
        }

        private void StartClient(RequestMessage request)
        {
            // Data buffer for incoming data.  
            byte[] bytes = new byte[1024];

            client = new TcpClient();
       //     client.SendBufferSize = Int32.MaxValue;
            try
            {
                Console.WriteLine("connecting to " + Ip +":"+ RequestedPort.ToString());
                client.Connect(Ip, RequestedPort);
                Console.WriteLine("connected");
                client.ReceiveBufferSize = Int32.MaxValue;
                networkStream = client.GetStream();
               
                string jsonStr = MessageParser.ToJson(request);//JsonConvert.SerializeObject(request);
                byte[] byteArrayToSend = Encoding.Default.GetBytes(jsonStr);

                networkStream.Write(byteArrayToSend, 0, byteArrayToSend.Length);
                networkStream.Flush();
                ListenForRequestAnswer();

            }
            catch (ArgumentNullException ane)
            {
                Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
            }
            catch (SocketException se)
            {
                Console.WriteLine("SocketException : {0}", se.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine("Unexpected exception : {0}", e.ToString());
            }     
        }
        public void StartClientThread(RequestMessage request)
        {
            Runner.Secondary(() => StartClient(request));
        }
    }
}