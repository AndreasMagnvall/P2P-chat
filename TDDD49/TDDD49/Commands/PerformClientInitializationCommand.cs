using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TDDD49.ViewModels;

namespace TDDD49.Commands
{
    public class PerformClientInitializationCommand: ICommand
    {
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        SetupViewModel viewModel;

        public PerformClientInitializationCommand(SetupViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public PerformClientInitializationCommand()
        {
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            viewModel.ShowDialogBoxClient();
        }
    }
}
