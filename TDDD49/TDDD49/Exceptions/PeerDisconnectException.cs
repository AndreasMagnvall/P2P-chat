using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDDD49.Exceptions
{
    public class PeerDisconnectException : Exception
    {
        public PeerDisconnectException()
        {
        }

        public PeerDisconnectException(string message) : base(message)
        { }

        public PeerDisconnectException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
