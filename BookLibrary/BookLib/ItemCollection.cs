using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BookLibServices;
using System.Globalization;
using System;

namespace BookLibDAL
{
    public class ItemCollection : List<AbstractItem>
    {
        private static ItemCollection instance;
        private static List<ISBN> registeredISBNs = new List<ISBN>();
        private static Dictionary<string, List<int>> registeredItemCopies = new Dictionary<string, List<int>>();
        private ItemCollection()
        {
        }

        public static ItemCollection Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ItemCollection();
                }
                return instance;
            }
        }

        public List<AbstractItem> Items
        {
            get
            {
                return this.ToList<AbstractItem>();
            }
        }

        public List<AbstractItem> getItemsByAuthor(string author)
        {
            return this.FindAll(x => x.Author.ToLower().Contains(author.ToLower()));
        }

        public List<AbstractItem> this[ISBN isbn]
        {
            get
            {
                string isbnNumber;
                if (isbn.Number.Length <= 11)
                {
                    isbnNumber = isbn.Number;
                }
                else
                {
                    throw new InvalidSerialNumberException("Invalid serial number!");
                }
                List<AbstractItem> foundItems = this.Where<AbstractItem>(x => x.ISBN.Number.Contains(isbn.Number)).ToList<AbstractItem>();
                return foundItems;
            }
        }

        public List<AbstractItem> this[string name]
        {
            get
            {
                return this.FindAll(x => x.Name.ToLower().Contains(name.ToLower()));
            }
        }

        public static List<ISBN> RegisteredISBNList
        {
            get
            {
                return registeredISBNs;
            }
        }
    }
}
