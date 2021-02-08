using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDDD49.Models.Messages
{
    public class RequestMessage : Message
    {
        public bool answer;
        public RequestMessage(bool answer, string message, string sender, string date) : base(message, sender, date) {
            this.answer = answer;
        }
    }
}
