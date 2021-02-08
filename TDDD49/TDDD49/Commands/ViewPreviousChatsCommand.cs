using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TDDD49.ViewModels;


namespace TDDD49.Commands
{
    public class ViewPreviousChatsCommand: ICommand
    {

        SetupViewModel viewModel;

        public ViewPreviousChatsCommand(SetupViewModel viewModel)
        {
            this.viewModel = viewModel;
        }
        public bool CanExecute(object parameter)
        {
            return true;
        }
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
            Console.WriteLine("execute");
            viewModel.ViewPreviousChats();
        }
    }
}

