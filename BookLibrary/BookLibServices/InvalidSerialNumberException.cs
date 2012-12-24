using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BookLibServices
{
    public class InvalidSerialNumberException : Exception
    {
        public InvalidSerialNumberException(string message) : base(message)
        {

        }
    }
}
