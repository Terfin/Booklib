using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BookLibServices
{
    [NameAttr(Desc = "Regular Journal")]
    public class RegularJournal : Journal
    {
        public enum Categories
        {
            [NameAttr(Desc = "Finance & Economy")]
            FinanceEconomy,
            Housekeeping,
            Fashion,
            Arts,
            Motor,
            NewsPaper,
            Celebrity
        }

        public RegularJournal(Dictionary<ValidItemParams, string> parameters, DateTime printDate, string subject, Categories cat)
            :base(parameters, printDate, subject)
        {
            this.Category = cat;
        }

        public Categories Category { get; set; }
    }
}
