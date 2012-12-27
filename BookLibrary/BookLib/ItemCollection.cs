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
        private static List<string> RegisteredISBNs = new List<string>();
        private ItemCollection()
        {
            this.Add(new ChildrenBook(new Dictionary<ValidItemParams, string>()
                        {
                            { ValidItemParams.Name, "foobar" },
                            { ValidItemParams.Author, "moshe" },
                            { ValidItemParams.EditionNumber, "1" },
                            { ValidItemParams.Location, "D:22"}
                        }, DateTime.MinValue, ChildrenBook.Categories.HistoricalFiction, true));
            this.Add(new ChildrenBook(new Dictionary<ValidItemParams, string>()
                        {
                            { ValidItemParams.Name, "foobar" },
                            { ValidItemParams.Author, "moshe" },
                            { ValidItemParams.EditionNumber, "1" },
                            { ValidItemParams.Location, "D:22"},
                            { ValidItemParams.ISBN, this[0].ISBN.Number }
                        }, DateTime.MinValue, ChildrenBook.Categories.HistoricalFiction, true));
            this.Add(new RegularJournal(new Dictionary<ValidItemParams, string>()
                        {
                            {ValidItemParams.Name, "fooptard"},
                            {ValidItemParams.Author, "goopar"},
                            {ValidItemParams.EditionNumber, "2"},
                            {ValidItemParams.Location, "A:35"}
                        }, DateTime.MinValue, "The great cornholio", RegularJournal.Categories.FinanceEconomy));
            ISBN.ISBNRegistrationChanged += ApproveISBNRegistration;
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

        public void ApproveISBNRegistration(object sender, EventArgs e)
        {
            if (RegisteredISBNs.Contains(((ISBN)sender).Number))
            {
                throw new InvalidSerialNumberException("Serial number already exists! Make sure you've typed the correct number!");
            }
            else
            {
                RegisteredISBNs.Add(((ISBN)sender).Number);
            }
        }

    }
}
