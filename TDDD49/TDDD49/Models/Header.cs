using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDDD49.Models
{
    public enum MessageType { 
        TextMessage,
        ImageMessage,
        BuzzMessage,
        PollMessage
    }

    public class Header
    {
        public int size;
        public MessageType messageType;

        public Header(int size, MessageType messageType)
        {
            this.size = size;
            this.messageType = messageType;
        }
    }
}
