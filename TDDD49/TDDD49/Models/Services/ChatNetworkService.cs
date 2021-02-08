using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDDD49.Sockets;
using TDDD49.Models;
using TDDD49.Models.Messages;
using TDDD49.Utility;
using System.Media;
using TDDD49.Properties;
using System.IO;
using TDDD49.Exceptions;

namespace TDDD49.Models.Services
{
    public class ChatNetworkService : Service
    {
        BaseSocket socket;
      

        public ChatNetworkService(BaseSocket socket, ConversationInfo info): base(info) // string username
        {
            this.socket = socket;

            socket.OnMessageRecieved += OnMessageRecievedHandler;
            socket.OnSocketDisconnect += OnDisconnectHandler;
            socket.OnMessageParseException += OnMessageParseExceptionHandler;
            //socket.OnSocketDisconnect += () =>
            //{
            //    OnServiceDisconnect?.Invoke();
            //};
        }

        private void OnMessageParseExceptionHandler(MessageParseException ex)
        {
            OnCorruptDataExceptionHandler(ex);
        }

        public override void Start()
        {
            Database.DatabaseManager.WriteInfo(info);
            Database.DatabaseManager.WriteMessages(Messages.ToList(), info);
            socket.StartReadingMessages();        
        }

        public void CloseSocket() 
        {
            socket.ClosePeerConnection();
        }

        public void OnMessageRecievedHandler(Message message, MessageType type)
        {
            switch (type) 
            {
                case MessageType.BuzzMessage:
                    OnBuzzMessageRecievedHandler((BuzzMessage)message);
                    break;

                case MessageType.TextMessage:
                    OnTextMessageRecievedHandler((TextMessage)message);
                    break;

                case MessageType.ImageMessage:
                    OnImageMessageRecievedHandler((ImageDataMessage)message);
                    break;
                
                default:
                    break;
            }

        }

        private void OnImageMessageRecievedHandler(ImageDataMessage message)
        {
           string path = Database.DatabaseManager.WriteImage(message, info);
           AddMessage(new ImageMessage(path, message.Sender));
           Database.DatabaseManager.WriteMessages(Messages.ToList(), info);
        }

        private void OnTextMessageRecievedHandler(TextMessage message)
        {
            AddMessage(message);
            Database.DatabaseManager.WriteMessages(Messages.ToList(), info);
            Console.WriteLine(Messages.Count);
            Console.WriteLine(message + "SASADAS");
        }

        private void OnBuzzMessageRecievedHandler(BuzzMessage message)
        {


           
            AddMessage(message);
            Database.DatabaseManager.WriteMessages(Messages.ToList(), info);
            OnBuzzHandler();
        }


        public void SendBuzzMessage()
        {
            BuzzMessage meBuzz = new BuzzMessage("You Buzzed " + info.PeerUsername, info.MyUsername);
            AddMessage(meBuzz);
            Database.DatabaseManager.WriteMessages(Messages.ToList(), info);


            BuzzMessage buzz = new BuzzMessage(info.MyUsername + " Buzzed you!", info.MyUsername);
            Runner.Secondary(() => socket.SendMessage(buzz, MessageType.BuzzMessage));
        }

        public void SendTextMessage(string messageContent)
        {
            TextMessage textMessage = new TextMessage(messageContent, info.MyUsername);
            Console.WriteLine("sneding message");
            //        TextMessage textMessage = new TextMessage(MessageValue, username);
            AddMessage(textMessage);
            Database.DatabaseManager.WriteMessages(Messages.ToList(), info);
            Runner.Secondary(() => socket.SendMessage(textMessage, MessageType.TextMessage));
            //socket.SendTextMessage(textMessage);
        }

        public void SendImageMessage(byte[] data, string extension)
        {

            ImageDataMessage imageDataMessage = new ImageDataMessage(Convert.ToBase64String(data), extension, info.MyUsername);
            string path = Database.DatabaseManager.WriteImage(imageDataMessage, info);
            AddMessage(new ImageMessage(path, info.MyUsername));
            Database.DatabaseManager.WriteMessages(Messages.ToList(), info);
            Runner.Secondary(() => socket.SendMessage(imageDataMessage, MessageType.ImageMessage));
           //socket.SendImageMessage(imageDataMessage)

        }
    }
}
