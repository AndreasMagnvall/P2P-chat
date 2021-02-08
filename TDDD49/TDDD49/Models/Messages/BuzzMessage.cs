using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDDD49.Models.Messages
{
    public class BuzzMessage : TextMessage
    {
        public BuzzMessage(string text, string sender): base(text, "System", sender, "1996")
        {

        }

        public BuzzMessage() : base("Buzz","System", "Sender" , "1996")
        {

        }
    }
}
