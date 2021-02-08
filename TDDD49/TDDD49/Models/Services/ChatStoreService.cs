using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDDD49.Models.Services
{
    class ChatStoreService : Service
    {
        public ChatStoreService(ConversationInfo info) : base(info)
        {

        }
        public override void Start()
        {
            ReadMessages();
        }

        private void ReadMessages()
        {
            ObservableCollection<Message> messages = new ObservableCollection<Message>(Database.DatabaseManager.ReadMessages(info));

            this.Messages = messages;
        }

    }
}
