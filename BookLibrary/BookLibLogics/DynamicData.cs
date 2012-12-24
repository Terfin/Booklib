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
        public int foo = 0;
 
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

        public ISearchStatusNotifier SearchWindowNotifier 
        {
            set
            {
                searchWindowNotifier = value;
            }
        }

        public ISearchStatusNotifier ResultsWindowNotifier
        {
            set
            {
                resultsWindowNotifier = value;
            }
        }

        public AbstractItem GetItemFromDataRow(DataRow row)
        {
            string rawSerial = row["ISSN|ISBN"].ToString();
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
        
    }
}
