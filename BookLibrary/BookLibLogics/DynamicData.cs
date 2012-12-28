using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BookLibDAL;
using BookLibServices;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;

namespace BookLibLogics
{
    /*
     * This is the first part of the logic behind the program.
     * It deals with doing all the backend operations of the search, edit, add and remove action,
     * while keeping data integrity.
     */
    public class DynamicData
    {
        /*
         * The class is built in a singleton pattern,
         * since we want to share this class' instance with the entire
         * program, we have to make it either static or a singleton.
         * I have chosen a singleton because static is a bit limiting
         * in the regard of what one can do
         */
        private static DynamicData instance;
        private ItemCollection coll = ItemCollection.Instance;
        public delegate List<AbstractItem> SearchFunction(string parameter, List<AbstractItem> items);
        public delegate void SearchNotifier(List<AbstractItem> items);
        public event SearchNotifier onSearchComplete;
 
        private DynamicData()
        {
        }

        public static DynamicData Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DynamicData();
                }
                return instance;
            }
        }


        public AbstractItem GetItemFromDataRow(DataRow row) //Gets a row from the table behind the results datagrid and parses it into an abstract item
        {
            string rawSerial = row["ISBN"].ToString();
            string typeString = row["Subtype"].ToString().Replace(" ", string.Empty);
            typeString = string.Format("BookLibServices.{0}, BookLibServices, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", typeString);
            Type type = Type.GetType(typeString);
            return coll[new ISBN(rawSerial)][0];
        }

        // Set of remove methods, one can remove item from the collection, completely, by simply providing a row from the datatable or an actual AbstractItem instance

        public void RemoveItem(DataRow row) 
        {
            AbstractItem item = GetItemFromDataRow(row);
            RemoveItem(item);
        }


        public void RemoveItem(AbstractItem item)
        {
            while (coll[item.ISBN].Count > 0)
            {
                coll.Remove(item);
            }
        }


        public void BorrowItem(DataRow dr)
        {
            /* A method to remove a copy of the item from the collection, thus, borrowing a copy.
             * Do note, one copy of the item is always left in the collection as it should appear in the search results
             * even if there are no copies left
             */
            AbstractItem item = GetItemFromDataRow(dr);
            if (coll.Contains(item) && coll[item.ISBN].Count > 1)
            {
                coll.Remove(item);
            }
            Search(item);
        }

        // Set of search methods, each for a different ocassion.

        public void Search(List<List<string>> listOfSearchValues, List<SearchFunction> searchFunctions)
        {
            /* 1. Complex search method.
             * This method receives a list of lists of strings and a list of SearchFunction (delegate)
             * The reason for the use of list of lists is to keep generality and keep the code short.
             * Regarding complexity, this theoretically can reach O(n^2), but is most unlikely.
             * The method runs the appropriate search function for the appropriate set of values.
             * The use of the hashset is to handle duplicate entries in collection.
             */
            HashSet<AbstractItem> results = null;
            for (int i = 0; i < searchFunctions.Count; i++)
            {
                List<AbstractItem> funcResults = new List<AbstractItem>();
                foreach (string value in listOfSearchValues[i])
                {
                    List<AbstractItem> lastSearchResults = results != null ? results.ToList() : new List<AbstractItem>();
                    funcResults.AddRange(searchFunctions[i](value, lastSearchResults));
                }
                results = new HashSet<AbstractItem>();
                results.UnionWith(funcResults);
            }
            if (onSearchComplete != null)
            {
                onSearchComplete(results.Distinct().ToList());
            }
        }

        public void Search()
        {
            /* 2. All items search method.
             * This method simply returns all the items in the collection, without duplicates.
             */
            HashSet<AbstractItem> results = new HashSet<AbstractItem>(coll);
            if (onSearchComplete != null)
            {
                onSearchComplete(results.Distinct().ToList());
            }
        }

        public void Search(AbstractItem item)
        {
            /* 3. Search a specific, known item.
             * Does exactly what it should.
             */
            HashSet<AbstractItem> results = new HashSet<AbstractItem>(coll[item.ISBN]);
            if (onSearchComplete != null)
            {
                onSearchComplete(results.ToList());
            }
        }

        public void AddItem(AbstractItem item) // Add item to the collection. Both new item and an existing item.
        {
            if (!coll.Contains(item) || coll[item.ISBN][0].GetHashCode() != item.GetHashCode())
            {
                while (!validateISBN(item.ISBN))
                {
                    item.ISBN = new ISBN();
                }
                ItemCollection.RegisteredISBNList.Add(item.ISBN);
                coll.Add(item);
                coll.Add(item);
            }
            else
            {
                coll.Add(item);
            }
            Search(item);
        }

        public bool validateISBN(ISBN isbn) // validates that the ISBN doesn't exist already.
        {
            if (ItemCollection.RegisteredISBNList.Contains(isbn))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public void addISBN(ISBN isbn) // Adds an isbn to the list.
        {
            ItemCollection.RegisteredISBNList.Add(isbn);
        }

        public void removeISBN(ISBN isbn) // Removes an ISBN from the list.
        {
            ItemCollection.RegisteredISBNList.Remove(isbn);
        }
    }
}
