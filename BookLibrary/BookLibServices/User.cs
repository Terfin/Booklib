using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BookLibServices
{
    class User
    {
        List<AbstractItem> borrowedItems = new List<AbstractItem>();
        public User(string name)
        {
            this.Name = name;
            this.LibID = new Guid();
        }
        public string Name { get; private set; }
        public Guid LibID { get; private set; }
        public List<AbstractItem> BorrowedItems
        {
            get
            {
                return borrowedItems;
            }
        }
    }
}
