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
     * Mandatory parameters to be passed in the constructor: name, print date and author.
     * Optional parameters: edition, ISBN (an object by itself), location.
     * 
     */

    public abstract class AbstractItem
    {
        Random rnd = new Random();
        private static List<int> CreatedItemsIds = new List<int>();
        private int _edition;
        private int id;

        public AbstractItem(Dictionary<ValidItemParams, string> parameters, DateTime printDate)
        {
            this.Name = parameters[ValidItemParams.Name];
            this.Location = parameters[ValidItemParams.Location];
            this.Author = parameters[ValidItemParams.Author];
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
                {
                    Edition = _edition;
                }
                else
                {
                    throw new InvalidParameterException("Invalid edition number! Edition must be a number!");
                }
            }
            else
            {
                Edition = 1;
            }
            int id = rnd.Next(int.MaxValue);
            while (CreatedItemsIds.Contains(id))
            {
                id = rnd.Next(int.MaxValue);
            }
            this.id = id;
            CreatedItemsIds.Add(id);
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
            return id;
        }
    }
}
