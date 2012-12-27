using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BookLibServices
{
    /*
     * This class represents a book, it does mostly nothing, except inheriting from AbstractItem and it is abstract as well.
     * The book can be labeled as a bestseller, by default it is not a bestseller.
     */
    public abstract class Book : AbstractItem
    {
        public Book(Dictionary<ValidItemParams, string> parameters, DateTime printDate, bool bestSeller = false) : base(parameters, printDate)
        {
            IsBestseller = bestSeller;
        }

        public bool IsBestseller { get; set; }
    }
}
