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
        ItemCollection coll = ItemCollection.Instance;
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
            coll.Remove(item);
        }

        public void Search(List<List<string>> listOfSearchValues, List<SearchFunction> searchFunctions)
        {
            List<AbstractItem> results = null;
            for (int i = 0; i < searchFunctions.Count; i++)
            {
                List<AbstractItem> funcResults = new List<AbstractItem>();
                foreach (string value in listOfSearchValues[i])
                {
                    funcResults.AddRange(searchFunctions[i](value, results));
                }
                results = funcResults.ToList();
            }
            if (onSearchComplete != null)
            {
                onSearchComplete(results);
            }
        }

        public void AddItem(AbstractItem item)
        {
            coll.Add(item);
        }
    }
}
