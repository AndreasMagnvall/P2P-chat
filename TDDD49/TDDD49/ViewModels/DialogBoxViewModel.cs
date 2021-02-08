using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TDDD49.Commands;
using TDDD49.Models;
using TDDD49.Sockets;
using TDDD49.Views;
namespace TDDD49.ViewModels
{
    class DialogBoxViewModel : ViewModel
    {
        private Action<string> commandFunction;
        private DialogBox dialogBox;
        public string Username { get; set; }
        public bool usernameWarningVisible;
        public bool UsernameWarningVisible
        {
            get
            {
                return usernameWarningVisible;
            }
            set
            {
                usernameWarningVisible = value;
                OnPropertyChanged("UsernameWarningVisible");
            }
        }

        public DialogBoxViewModel(Action<string> func, DialogBox view)
        {
            commandFunction = func;
            dialogBox = view;
        }
        public ICommand SetUsernameCommand
        {
            get
            {
                return new RelayCommand(o => {
                    if (Username.Length == 0)
                    {
                        UsernameWarningVisible = true;
                        return;
                    }
                    commandFunction(Username);
                    dialogBox.Close();
                }, o => {
                    return true;
                });
            }
        }
    }
}
