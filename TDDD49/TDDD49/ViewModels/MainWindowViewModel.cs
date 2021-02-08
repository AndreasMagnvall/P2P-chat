using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TDDD49.Sockets;
using System.Collections.ObjectModel;
//using Microsoft.VisualStudio.PlatformUI;
using TDDD49.Models.Services;
using TDDD49.Models;
using TDDD49.Commands;

namespace TDDD49.ViewModels
{
    //https://social.technet.microsoft.com/wiki/contents/articles/30898.simple-navigation-technique-in-wpf-using-mvvm.aspx
    public class MainWindowViewModel : ViewModel
    {
        public ICommand OpenChatCommand  { get; set; }
        public ICommand CloseChatCommand { get; set; }
        public ICommand OpenSetupCommand { get; set; }
        public ICommand OpenConversationsCommand { get; set; }

        //https://rachel53461.wordpress.com/2011/12/18/navigation-with-mvvm-2/
        ICommand ChangePageCommand { get; set; }


       // private SyncClientSocket syncClientSocket;
       // private SyncServerSocket syncServerSocket;

        private ViewModel currentPageViewModel;

        public ViewModel CurrentPageViewModel
        {
            get
            {
                return currentPageViewModel;
            }
            set
            {
                if (currentPageViewModel != value)
                {
                    currentPageViewModel = value;
                    OnPropertyChanged("CurrentPageViewModel");
                }
            }
        }

        public MainWindowViewModel()
        {
            //      OpenChatCommand = new BaseCommand(OpenEmp);
            //    OpenSetupCommand = new BaseCommand(OpenDept);

            OpenConversationsCommand = new OpenConversationsCommand(this);
            OpenSetup();
        }

        //public void OpenSetup()
        //{
        //    OnPeerDisconnectedEventHandler();
        //}

        public void OpenConversationsView() 
        {
            ConversationsViewModel viewModel = new ConversationsViewModel(new ConversationsService());
            viewModel.OnSelectConversation += OpenChatWithConversation;

            CurrentPageViewModel = viewModel;
            
        }
        private void OpenChatWithConversation(ConversationInfo info)
        {
            ChatViewModel chatViewModel = new ChatViewModel(new ChatStoreService(info));
            CurrentPageViewModel = chatViewModel;
        }

        private void OnPeerDisconnectedEventHandler()
        {
            OpenSetup();
        }

        private void OnPeerAcceptFromServerEventHandler(ConversationInfo info, BaseSocket clientSock)
        {
            Console.WriteLine("server accepted");
            ChatViewModel chatViewModel = new ChatViewModel(new ChatNetworkService(clientSock, info));
            chatViewModel.OnPeerDisconnectedEvent += OnPeerDisconnectedEventHandler;
            CurrentPageViewModel = chatViewModel;

           
        }

        private void OnPeerAcceptFromClientEventHandler(ConversationInfo info, BaseSocket serverSock)
        {
            ChatViewModel chatViewModel = new ChatViewModel(new ChatNetworkService(serverSock, info));
            //NetworkChatViewModel chatViewModel = new NetworkChatViewModel(syncServerSocket, username);
            chatViewModel.OnPeerDisconnectedEvent += OnPeerDisconnectedEventHandler;
            CurrentPageViewModel = chatViewModel;

            Console.WriteLine("client accepted");
        }
        private void OpenSetup() 
        {

            SyncServerSocket syncServerSocket = new SyncServerSocket();
            SyncClientSocket syncClientSocket = new SyncClientSocket();
            var setupViewModel = new SetupViewModel(syncClientSocket, syncServerSocket);
            setupViewModel.OnPeerAcceptFromServer += OnPeerAcceptFromServerEventHandler;
            setupViewModel.OnPeerAcceptFromClient += OnPeerAcceptFromClientEventHandler;
            setupViewModel.OnViewPreviousChats    += OpenConversationsView;
            CurrentPageViewModel = setupViewModel;
        }

        private void ChangeViewModel(ViewModel viewModel)
        {
            CurrentPageViewModel = viewModel;
        }
    }
}
