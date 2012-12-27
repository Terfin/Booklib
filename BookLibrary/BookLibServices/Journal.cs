using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BookLibServices
{
    /*
     * The class represents a journal. Does nothing, inherits from AbstractItem and it is abstract by itself.
     * The only thing it has more is a subject which is not a mandatory parameter.
     */
    public abstract class Journal : AbstractItem
    {
        public Journal(Dictionary<ValidItemParams, string> parameters, DateTime printDate, string subject = "")
            : base(parameters, printDate)
        {
            this.Subject = subject;
        }
        
        public string Subject { get; set; }

    }
}
