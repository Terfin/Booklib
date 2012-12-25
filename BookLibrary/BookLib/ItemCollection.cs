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
        private ItemCollection()
        {
            this.Add(new ChildrenBook(new Dictionary<ValidItemParams, string>()
                        {
                            { ValidItemParams.Name, "foobar" },
                            { ValidItemParams.Author, "moshe" },
                            { ValidItemParams.EditionNumber, "1" },
                            { ValidItemParams.CopyNumber, "1" },
                            { ValidItemParams.Location, "D:22"}
                        }, DateTime.MinValue, ChildrenBook.Categories.HistoricalFiction));
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

    }
}
