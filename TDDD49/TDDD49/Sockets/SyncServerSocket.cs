using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.ComponentModel;
using TDDD49.Models;
using TDDD49.Models.Messages;
using System.Diagnostics;
using Newtonsoft.Json;
using System.Windows;
using TDDD49.Utility;
using System.Security.Permissions;

namespace TDDD49.Sockets
{
    public class SyncServerSocket : BaseSocket
    {
        //Events
        public delegate void OnStartListeningEvent();
        public event OnStartListeningEvent OnStartListening;

        public delegate void OnRequestEvent( RequestMessage message);
        public event OnRequestEvent OnRequest;
        private TcpListenerEx listener = null;
        private IPEndPoint localEndPoint;
        private Thread serverThread;

        public bool IsListening { get {return (listener?.Active != null || false); }}

        public void SetEndPoint(int port)
        {
            localEndPoint = new IPEndPoint(IPAddress.Any, port);
        }

        public SyncServerSocket() {}


        protected void OnRequestReceived(RequestMessage message)
        {
            OnRequestEvent handler = OnRequest;
            Runner.Primary(() => handler?.Invoke(message));
        }

        public void StopListening()
        {
            listener?.Stop();   
        }

        private void AcceptConnection()
        {
            if (client.Connected)
            {
                StopListening();
                byte[] bytesFrom = new byte[REQUEST_SIZE];

                networkStream.Read(bytesFrom, 0, REQUEST_SIZE);
                networkStream.Flush();

                //trim removes not set parts of bytesfrom
                string msg = Encoding.Default.GetString(bytesFrom, 0, 1024).Trim('\0');

                RequestMessage message = MessageParser.FromJson<RequestMessage>(msg);
                Console.WriteLine(message);
                Console.WriteLine("OOOOOOOOOOOOO");

                OnRequestReceived(message);

                return;
                //TODO: EVENT SERVER CLIENT DISPATCH CLIENT WANTS TO TALK
            }
        }
        private void StartListening()
        {        
            try
            {
                listener = new TcpListenerEx(localEndPoint);             
                listener.Start();
                Runner.Primary(() => OnStartListening.Invoke());

                // An incoming connection needs to be processed.  
                client = listener.AcceptTcpClient();
                client.ReceiveBufferSize = Int32.MaxValue;
                networkStream = client.GetStream();

                AcceptConnection();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString() + "Line 79");
            }


            Console.WriteLine("\nPress ENTER to continue...");
            //Console.Read();
        }

        [SecurityPermissionAttribute(SecurityAction.Demand, ControlThread = true)]
        public void StartServerThread()
        {

            serverThread?.Abort();
            serverThread = Runner.Secondary(() => StartListening());
        }
    }
}