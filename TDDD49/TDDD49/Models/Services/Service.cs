using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using TDDD49.Exceptions;
using TDDD49.Models;
using TDDD49.Models.Services;

namespace TDDD49.Models.Services
{
    public abstract class Service : IService
    {
        public ObservableCollection<Message> Messages { get; set; }
        Thread thread;


        protected readonly ConversationInfo info;
        public delegate void OnServiceDisconnectEvent();
        public delegate void OnBuzzEvent();

        public event OnServiceDisconnectEvent OnServiceDisconnect;
        public event OnBuzzEvent OnBuzz;

        public delegate void OnCorruptDataExceptionEvent(Exception ex);
        public event OnCorruptDataExceptionEvent OnCorruptDataException;

        public Service(ConversationInfo info)
        {
            Messages = new ObservableCollection<Message>();
            this.info = info;
        }
        protected void OnCorruptDataExceptionHandler(Exception ex)
        {
            this.OnCorruptDataException?.Invoke(ex);
        }
        protected void OnDisconnectHandler()
        {
            this.OnServiceDisconnect?.Invoke();
        }
        protected void OnBuzzHandler()
        {
            this.OnBuzz?.Invoke();
        }

        protected void AddMessage(Message message)
        {
            // https://stackoverflow.com/questions/2091988/how-do-i-update-an-observablecollection-via-a-worker-thread
            this.Messages.Add(message);
        }

        public abstract void Start();
    }
}
