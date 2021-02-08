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
using TDDD49.Views;
using TDDD49.ViewModels;
//using TDDD49.Sockets;

namespace TDDD49 
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            
            InitializeComponent();
            // this.Content = "/Views/SetupView";

            this.DataContext = new MainWindowViewModel();
            //vm.OpenSetup();
            //SetupView page = new SetupView();

            //MainFrame.Navigate(page);


        }
    }
}
