using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDDD49.Models
{
    public class TextMessage : Message
    {
        public string Text { get; set; }
        public TextMessage(string text, string message, string sender, string date): base(message, sender, date)
        {
            this.Text = text;
        }
        public TextMessage(string text, string sender) : base("Ok", sender, "1996")
        {
            this.Text = text;
        }
        public TextMessage() : base("Ok", "Jag", "1996")
        {

        }
    }
}
