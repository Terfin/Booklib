using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BookLibServices
{
    /*
     * The class represents a journal. Does nothing, inherits from AbstractItem and it has an ISSN (International Standard Serial Number)
     */
    public class Journal : AbstractItem
    {
        public Journal(Dictionary<ValidItemParams, string> parameters, DateTime printDate, string subject)
            : base(parameters, printDate)
        {
            this.Subject = subject;
        }
        
        public string Subject { get; set; }

    }
}
