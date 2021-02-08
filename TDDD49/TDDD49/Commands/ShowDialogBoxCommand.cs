using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TDDD49.ViewModels;

namespace TDDD49.Commands
{
    public class ShowDialogBoxCommand : ICommand
    {

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        SetupViewModel viewModel;
        public ShowDialogBoxCommand(SetupViewModel viewModel)
        {
            this.viewModel = viewModel;
            Console.WriteLine("created");
        }

        public ShowDialogBoxCommand()
        {
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            Console.WriteLine("show");
            viewModel.ShowDialogBox();
        }
    }
}
