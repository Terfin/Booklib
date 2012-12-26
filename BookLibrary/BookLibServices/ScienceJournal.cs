using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BookLibServices
{
    [NameAttr(Desc = "Science Journal")]
    public class ScienceJournal : Journal
    {
        public enum Categories
        {
            Psychology,
            Medicine,
            Biology,
            Physics,
            Chemistry,
            Engineering,
            [NameAttr(Desc = "Computer Sciences")]
            ComputerSciences,
            Philosophy
        }

        public ScienceJournal(Dictionary<ValidItemParams, string> parameters, DateTime printDate, string subject, Categories cat)
            : base(parameters, printDate, subject)
        {
            this.Category = cat;
        }

        public Categories Category { get; set; }
    }
}
