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
using TDDD49.ViewModels;

namespace TDDD49.Views
{
    /// <summary>
    /// Interaction logic for ChatViewModel.xaml
    /// </summary>
    public partial class ChatView : UserControl
    {
        // ItemTemplate="{StaticResource MessageTemplate}"
        ////DataContext="{Binding Source={StaticResource ChatViewModel}}"
        public ChatView()
        {
            InitializeComponent();
        }

        public void Grid_Drop(object sender, DragEventArgs e)
        {
            ChatViewModel viewModel = (ChatViewModel)DataContext;
            viewModel.GridDropCommand.Execute(e);
            //OnGridDrop?.Invoke(sender, e);
        }
    }
}
