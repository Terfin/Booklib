using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BookLibServices
{
    class OutOfStockException : Exception
    {
        public OutOfStockException(string message) : base(message)
        {

        }
    }
}
