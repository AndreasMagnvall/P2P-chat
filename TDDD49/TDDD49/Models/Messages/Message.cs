using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDDD49.Models
{
    public abstract class Message
    {
        public string MessageContent { get; set; }
        public string Sender { get; set; }
        public string Date { get; set; }

        public Message(string message, string sender, string date) {
            this.MessageContent = message;
            this.Sender = sender;
            this.Date = date;
        }
    }
}
