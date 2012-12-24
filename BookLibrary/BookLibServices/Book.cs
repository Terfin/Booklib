using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BookLibServices
{
    /*
     * This class represents a book, it does mostly nothing, except inheriting from AbstractItem and it has an ISBN (International Standard Book Number)
     */
    public class Book : AbstractItem
    {
        public Book(Dictionary<ValidItemParams, string> parameters, DateTime printDate, bool bestSeller = false) : base(parameters, printDate)
        {
            IsBestseller = bestSeller;
        }

        public bool IsBestseller { get; set; }
    }
}
