﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BookLibServices
{
    public class InvalidParameterException : Exception
    {
        public InvalidParameterException(string message) : base(message)
        {
        }
    }
}
