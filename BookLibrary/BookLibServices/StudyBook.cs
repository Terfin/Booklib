using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BookLibServices
{
    [NameAttr(Desc = "Study Book")]
    public class StudyBook : Book
    {
        public enum Categories
        {
            Philosophy,
            Psychology,
            Biology,
            Biochemistry,
            Chemistry,
            Physics,
            Mathematics,
            Mechanics,
            [NameAttr(Desc = "Computer Sciences")]
            ComputerSciences
        }
        
        public StudyBook(Dictionary<ValidItemParams, string> parameters, DateTime printDate, Categories cat)
            : this(parameters, printDate, cat, false)
        {
        }

        public StudyBook(Dictionary<ValidItemParams, string> parameters, DateTime printDate, Categories cat, bool bestseller)
            : base(parameters, printDate, bestseller)
        {
            this.Category = cat;
        }

        public Categories Category { get; private set; }
    }
}
