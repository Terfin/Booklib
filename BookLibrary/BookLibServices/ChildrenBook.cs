using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BookLibServices
{
    [NameAttr(Desc = "Children Book")]
    public class ChildrenBook : Book
    {
        public enum Categories
        {
            PictureBook,
            Poetry,
            Folklore,
            Fantasy,
            [NameAttr(Desc = "Science Fiction")]
            ScienceFiction,
            [NameAttr(Desc = "Real Fiction")]
            RealFiction,
            [NameAttr(Desc = "Historical Fiction")]
            HistoricalFiction,
            Biography,
            NonFiction
        }

        public ChildrenBook(Dictionary<ValidItemParams, string> parameters, DateTime printTime, Categories cat)
            : this(parameters, printTime, cat, false)
        {
        }

        public ChildrenBook(Dictionary<ValidItemParams, string> parameters, DateTime printTime, Categories cat, bool bestseller)
            :base(parameters, printTime, bestseller)
        {
            this.Category = cat;
        }

        public Categories Category { get; set; }
    }
}
