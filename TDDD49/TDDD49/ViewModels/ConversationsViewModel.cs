using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TDDD49.Commands;
using TDDD49.Models;
using TDDD49.Models.Services;

namespace TDDD49.ViewModels
{
    public class ConversationsViewModel : ViewModel
    {
        public delegate void SelectConversationEvent(ConversationInfo info);
        public event SelectConversationEvent OnSelectConversation;

        private string searchTerm;
        public string SearchTerm
        {
            get
            {
                return searchTerm;
            }
            set
            {

                if (value.Length > 0)
                {
                    SearchEnabled = true;

                    OnPropertyChanged("SearchEnabled");
                }
                else
                {
                    SearchEnabled = false;
                    Service.SetSearchResultToAll();
                    OnPropertyChanged("SearchEnabled");
                }
                searchTerm = value;
            }
        }
        public ConversationsService Service { get; set; }

        public bool SearchEnabled { get; set; }
        //public List<ConversationInfo> conversations;

        public ConversationsViewModel(ConversationsService service)
        {
            Service = service;
            service.Start();
            SearchEnabled = false;
        }

        public ICommand SearchCommand
        {
            get
            {
                return new RelayCommand(
                    (param) =>
                    {
                        if (SearchTerm.Length > 0)
                        {
                            Console.WriteLine("searching");
                            Service.SearchConversation(SearchTerm);
                            OnPropertyChanged("Service.Conversations");
                        }
                        else
                        {

                            Service.SetSearchResultToAll();
                        }
                    },

                    (o) =>
                    {
                        return true;
                    });
            }
        }


        public ICommand SelectConversationCommand
        {

            get
            {
                return new RelayCommand(
                    (param) =>
                    {
                        Console.WriteLine("woooo");
                        // PLOCKA ut parameter
                        ConversationInfo info = (ConversationInfo)param;
                        OnSelectConversation.Invoke(info);
                    },

                    (o) =>
                    {
                        return true;
                    });

            }
        }




        public ICommand SearchTypeCommand
        { 
            get
            {
                return new RelayCommand(
                    (param) =>
                    {
                        if (SearchTerm.Length > 0)
                        {
                            OnPropertyChanged("SearchEnabled");
                        }
                    },

                    (o) =>
                    {
                        return true;
                    });
            }
        }
    }
}
