using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDDD49.Exceptions
{
    public class CorruptDataException : Exception
    {
        public CorruptDataException()
        {
        }

        public CorruptDataException(string message) : base(message)
        { }

        public CorruptDataException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
