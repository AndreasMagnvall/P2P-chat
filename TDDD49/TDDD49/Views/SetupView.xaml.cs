using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TDDD49.Sockets;
using System.Threading;
using TDDD49.ViewModels;
namespace TDDD49.Views
{
    /// <summary>
    /// Interaction logic for setup.xaml
    /// </summary>
    public partial class SetupView : UserControl
    {

        /*
                TextBox serverPortTextBox;
                TextBox ipTextBox;
                TextBox portTextBox;
          */
      //  SyncServerSocket serverSocket;
        public SetupView()
        {
            InitializeComponent();
            //this.ipTextBox = ip_textbox;
            //this.portTextBox = port_textbox;
            //this.serverPortTextBox = server_port_textbox;

            //string ip = ipTextbox.Text;
            //string port = portTextbox.Text;



        }
        /*
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            String destinationPortStr = port_textbox.Text;
            String serverPortStr = server_port_textbox.Text;
            //Console.WriteLine("Enter listen port for");
            //String portStr = Console.ReadLine();
            serverSocket;


            int portNumber;// = Convert.ToInt32(serverPort);
            if(int.TryParse(serverPortStr, out portNumber))
            {
                SyncServerSocket.StartServerThread(portNumber);
            }



            //Console.WriteLine("Enter The destination port");
            //String portString = Console.ReadLine();
            int requestedPort;
            if (int.TryParse(destinationPortStr, out requestedPort))//destinationPort != "")
            {
                //int requestedPort = Convert.ToInt32(destinationPortStr); // int.Parse(portString);
                SyncClientSocket.StartClientThread(requestedPort);
            }
            */
        }
    
}
