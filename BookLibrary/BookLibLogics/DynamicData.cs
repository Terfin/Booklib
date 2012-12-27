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
    public class DynamicData
    {
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

        public AbstractItem GetItemFromDataRow(DataRow row)
        {
            string rawSerial = row["ISBN"].ToString();
            string typeString = row["Subtype"].ToString().Replace(" ", string.Empty);
            typeString = string.Format("BookLibServices.{0}, BookLibServices, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", typeString);
            Type type = Type.GetType(typeString);
            return coll[new ISBN(rawSerial)][0];
        }

        public void RemoveItem(DataRow row)
        {
            try
            {
                AbstractItem item = GetItemFromDataRow(row);
                coll.Remove(item);
            }
            catch (NullReferenceException e)
            {
                throw e;
            }
            
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
            AbstractItem item = GetItemFromDataRow(dr);
            if (coll.Contains(item))
            {
                coll.Remove(item);
            }
            Search();
        }

        public void Search(List<List<string>> listOfSearchValues, List<SearchFunction> searchFunctions)
        {
            HashSet<AbstractItem> results = null;
            for (int i = 0; i < searchFunctions.Count; i++)
            {
                List<AbstractItem> funcResults = new List<AbstractItem>();
                foreach (string value in listOfSearchValues[i])
                {
                    funcResults.AddRange(searchFunctions[i](value, results.ToList()));
                }
                results.UnionWith(funcResults);
            }
            if (onSearchComplete != null)
            {
                onSearchComplete(results.Distinct().ToList());
            }
        }

        public void Search()
        {
            HashSet<AbstractItem> results = new HashSet<AbstractItem>(coll);
            if (onSearchComplete != null)
            {
                onSearchComplete(results.Distinct().ToList());
            }
        }

        public void AddItem(AbstractItem item)
        {
            coll.Add(item);
        }
    }
}
