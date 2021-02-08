using System;  
using System.Net;  
using System.Net.Sockets;  
using System.Text;
using System.ComponentModel;
using TDDD49.Sockets;
using TDDD49.Commands;
using TDDD49.Models;
using System.Windows;
using TDDD49.Models.Messages;
using TDDD49.Views;

namespace TDDD49.ViewModels
{
    public class SetupViewModel : ViewModel
    {
        private DialogBox dialogBox;

        public delegate void PeerAcceptedEvent(ConversationInfo info, BaseSocket socket);
        public delegate void PeerRejectEvent();
        public delegate void ViewPreviousChatsEvent();

        public event PeerAcceptedEvent OnPeerAcceptFromServer;
        public event PeerAcceptedEvent OnPeerAcceptFromClient;

        public event PeerRejectEvent OnPeerRejectFromServer;
        public event PeerRejectEvent OnPeerRejectFromClient;

        public event ViewPreviousChatsEvent OnViewPreviousChats;

        internal void ViewPreviousChats()
        {
            OnViewPreviousChats.Invoke();
        }

        public SendInviteCommand SendInviteCommand { get; set; }
        public StartServerCommand StartServerCommand { get; set; }
        public ShowDialogBoxCommand ShowDialogBoxCommand { get; set; }
        public ViewPreviousChatsCommand ViewPreviousChatsCommand { get; set; }


        public PerformClientInitializationCommand PerformClientInitializationCommand { get; set; }

      //  private string ipTextbox;
        public bool StartServerButtonEnabled { 
            get 
            {             
                return !syncServerSocket.IsListening;
            }   
        }
        public string IpTextbox
        {
            get; set;
        }
        
        private string currentUsername;

        public string ServerPortTextbox  { get; set; }
        public string RequestPortTextbox { get; set; }
        private bool isConnected;

        private SyncClientSocket syncClientSocket;
        private SyncServerSocket syncServerSocket;
        //accepty deny request
        public bool IsConnected {
            get { return isConnected;  }
            set {
                isConnected = value;
                OnPropertyChanged("IsConnected");
            }
        }

        public SetupViewModel(SyncClientSocket syncClientSocket, SyncServerSocket syncServerSocket)
        {
            isConnected = false;   
            SendInviteCommand                  = new SendInviteCommand(this);
            StartServerCommand                 = new StartServerCommand(this);
            ShowDialogBoxCommand               = new ShowDialogBoxCommand(this);
            ViewPreviousChatsCommand           = new ViewPreviousChatsCommand(this);
            //OpenConversationsCommand           = new OpenConversationsCommand(this);
            PerformClientInitializationCommand = new PerformClientInitializationCommand(this);
            this.syncClientSocket = syncClientSocket;
            this.syncServerSocket = syncServerSocket;
            registerHandlers();
        }



        private void registerHandlers() {
            syncClientSocket.OnPeerAccepted += (RequestMessage message) =>
            {
                Console.WriteLine("breakpoint OnPeerAccepted");
                OnPeerAcceptFromServer?.Invoke(new ConversationInfo(currentUsername, message.Sender, DateTime.Now.ToString()), syncClientSocket);
            };

            syncClientSocket.OnPeerRejected += (RequestMessage message) =>
            {
                if (message.answer == false)
                {
                    MessageBox.Show("Other part denied request");
                }

            };
            syncServerSocket.OnStartListening += () => OnPropertyChanged("StartServerButtonEnabled");

        }

        public void StartServer()
        {
            try
            {
                syncServerSocket.SetEndPoint(int.Parse(ServerPortTextbox));
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                MessageBox.Show("Error parsing ip");
            }
            syncServerSocket.OnRequest += OnRequestReceivedFromClient;
            syncServerSocket.StartServerThread();

            Console.WriteLine("Starting server");
        }

        public void ShowDialogBox()
        {
            Console.WriteLine("show");
            dialogBox = new DialogBox();
            dialogBox.DataContext = this;
            dialogBox.Show();
        }

        public void ShowDialogBoxClient()
        {
            Console.WriteLine("show");
            dialogBox = new DialogBox();
            dialogBox.DataContext = new DialogBoxViewModel(username =>
            {
                
                SendInvite(username);
            }, dialogBox);
            dialogBox.Show();
        }

        public void ShowDialogBoxServer(Action<string> action)
        {
            Console.WriteLine("show");
            dialogBox = new DialogBox();
            dialogBox.DataContext = new DialogBoxViewModel(action, dialogBox);
            dialogBox.Show();
        }
        private void OnRequestReceivedFromClient(RequestMessage message) 
        {
            MessageBoxResult result = MessageBox.Show(message.MessageContent, message.Sender, MessageBoxButton.YesNo, MessageBoxImage.Question);

            //https://social.technet.microsoft.com/wiki/contents/articles/30898.simple-navigation-technique-in-wpf-using-mvvm.aspx
            Console.Write("SAOOSADOSADOODSAODSAOSAD  ");
            Console.WriteLine(result);
            if (result == MessageBoxResult.Yes)
            {
                ShowDialogBoxServer((username) =>
                {
                    this.syncServerSocket.SendAnswer(new RequestMessage(true, "Vill konnekta med dig din jävel", username, "IDAG"));
                    syncServerSocket.StopListening();
                    OnPeerAcceptFromClient?.Invoke(new ConversationInfo(username, message.Sender, DateTime.Now.ToString()), syncServerSocket);
                });
            }
            else
            {
                this.syncServerSocket.SendAnswer(new RequestMessage(false, "Vill inte konnekta med dig din jävel", "Anonymous", "IDAG"));
                syncServerSocket.StartServerThread();
            }      
        }


        public void SendInvite(string username)
        {

            currentUsername = username;

            try
            {
                syncClientSocket.Ip = IpTextbox;
                syncClientSocket.RequestedPort = int.Parse(RequestPortTextbox);
                RequestMessage request = new RequestMessage(true, username + " Vill Connecta med dig", username, "imorgon");
                syncClientSocket.StartClientThread(request);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}