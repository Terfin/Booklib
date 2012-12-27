using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;

namespace BookLibServices
{
    /*
     * This is the base class of the project, AbstractItem, it is abstract and thus cannot be instantiated
     * Every item, whether it is a book or a journal, has at least a name, print date, category, identifying number and a number of copies that currently exist in the library
     * 
     */

    public abstract class AbstractItem
    {
        private int _edition;
        private static List<AbstractItem> existingItems = new List<AbstractItem>();
        public AbstractItem(Dictionary<ValidItemParams, string> parameters, DateTime printDate)
        {
            this.Name = parameters[ValidItemParams.Name];
            this.PrintDate = printDate;
            if (parameters.ContainsKey(ValidItemParams.ISBN))
            {
                this.ISBN = new ISBN(parameters[ValidItemParams.ISBN]);
            }
            else
            {
                this.ISBN = new ISBN();
            }
            if (parameters.ContainsKey(ValidItemParams.EditionNumber))
            {
                if (int.TryParse(parameters[ValidItemParams.EditionNumber], out _edition))
                { }
                else
                {
                    throw new InvalidParameterException("Invalid edition number! Edition must be a number!");
                }
            }
            this.Location = parameters[ValidItemParams.Location];
            this.Author = parameters[ValidItemParams.Author];
            if (!existingItems.Contains(this))
            {
                existingItems.Add(this);
            }
        }


        public ISBN ISBN 
        { get; set; }
        public string Name { get;  set; }
        public DateTime PrintDate { get; set; }
        public string Location { get; set; }
        public string Author { get; set; }
        public int CopyNumber { get; set; }
        public int Edition { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is AbstractItem)
            {
                return this.ISBN.Number == ((AbstractItem)obj).ISBN.Number;
            }
            else
            {
                return this == obj;
            }
        }

        public override int GetHashCode()
        {
            string hashcode = this.ISBN.Number.Replace("-", string.Empty);
            return int.Parse(hashcode);
        }
    }
}
