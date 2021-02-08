using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TDDD49.Commands;
using TDDD49.Models;
using TDDD49.Models.Messages;
using TDDD49.Models.Services;
using TDDD49.Properties;

namespace TDDD49.ViewModels
{
    public class ChatViewModel : ViewModel
    {
        //public NetworkService Service = new NetworkService(null, null);
        public Service Service { get; set; }
        // private readonly ConversationInfo info;

        // EXTERNAL EVENTS START

        public delegate void PeerDisconnected();
        public event PeerDisconnected OnPeerDisconnectedEvent;

        // EXTERNAL EVENTS END

        public SendTextMessageCommand SendTextMessageCommand { get; set; }

        //    this.socket = socket;
        //    this.username = username;

        //    Messages = new ObservableCollection<Message>();

        //    socket.StartReadingMessages();
        //    socket.OnMessageRecieved += OnMessageRecievedHandler;
        //    socket.OnDisconnect += OnDisconnectHandler;
        //}

        //public void OnMessageRecievedHandler(Message message)

        public string MessageValue { get; set; }

        public ChatViewModel(Service service)
        {
            // this.info = info;
            this.Service = service;
            this.SendTextMessageCommand = new SendTextMessageCommand(this);
            this.Service.OnServiceDisconnect += OnServiceDisconnectHandler;
            this.Service.OnBuzz += OnBuzzHandler;
            this.Service.OnCorruptDataException += (ex) => MessageBox.Show(ex.Message);

            this.Service.Start();


        }


        public void OnBuzzHandler()
        {
            Stream str = Resources.BuzzSound2;
            SoundPlayer snd = new SoundPlayer(str);
            snd.Play();
        }

        public void SendTextMessage()
        {
            //TextMessage textMessage = new TextMessage(MessageValue, info.myUsername);
            // TODO: store the thing here
            ChatNetworkService networkService = (ChatNetworkService)Service;
            networkService.SendTextMessage(MessageValue);

            //AddMessage(textMessage);
        }

        public void SendImageMessage(byte[] data, string extension)
        {
            //ImageMessage imageMessage = new ImageMessage(data, info.myUsername);
            // TODO: store the thing here
            ChatNetworkService networkService = (ChatNetworkService)Service;

            networkService.SendImageMessage(data, extension);
        }

        public void OnServiceDisconnectHandler()
        {
            //OnPeerDisconnectedEventHandler();

            MessageBoxResult result = MessageBox.Show("Disconnected");
            if (result == MessageBoxResult.OK)
            {
                OnPeerDisconnectedEvent?.Invoke();
            }
        }

        public ICommand SendBuzzMessageCommand
        {
            get
            {
                return new RelayCommand((param) =>
                {
                    ChatNetworkService service = (ChatNetworkService)Service;
                    service.SendBuzzMessage();

                }, (param) =>
                {
                    return true;
                });

            }
        }

        public ICommand DisconnectCommand
        {
            get
            {
                return new RelayCommand((param) =>
                {
                    ChatNetworkService service = (ChatNetworkService)Service;
                    service.CloseSocket();
                    OnPeerDisconnectedEvent?.Invoke();
                }, (param) =>
                {
                    return true;
                });

            }
        }

        public ICommand GridDropCommand
        {
            get
            {
                return new RelayCommand((param) =>
                {
                    var e = (DragEventArgs)param;
                    var file = e.Data.GetData(DataFormats.FileDrop) as string[];

                    // string path = file[0];
                    foreach (string path in file)
                    {
                        string extension = Path.GetExtension(path);
                        Console.WriteLine("FILEN ÄR HÄR" + path);

                        byte[] fileBinary = File.ReadAllBytes(path);

                        Console.WriteLine("size " + fileBinary.Length);
                        string str = Encoding.Default.GetString(fileBinary, 0, fileBinary.Length);
                        Console.WriteLine(str.Length);
                        Console.WriteLine("ODK " + fileBinary.Length);

                        SendImageMessage(fileBinary, extension);
                    }

                }, (param) =>
                {
                    return true;
                });

            }

        }
    }
}