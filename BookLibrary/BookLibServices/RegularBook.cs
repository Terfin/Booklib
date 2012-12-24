using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BookLibServices
{
    [NameAttr(Desc="Regular Book")]
    public class RegularBook : Book
    {
        public enum Categories
        {
            [NameAttr(Desc = "Art & Design")]
            ArtDesign,
            AudioBook,
            Biography,
            Classic,
            Fiction,
            [NameAttr(Desc = "Science Fiction")]
            ScienceFiction,
            Travels
        }

        public RegularBook(Dictionary<ValidItemParams, string> parameters, DateTime printDate, Categories cat, bool bestseller) : base(parameters, printDate, bestseller)
        {
            this.Category = cat;
        }
        
        public RegularBook(Dictionary<ValidItemParams, string> parameters, DateTime printDate, Categories cat)
            : this(parameters, printDate, cat, false)
        {
            this.Category = cat;
        }
        public Categories Category { get; private set; }
    }
}
