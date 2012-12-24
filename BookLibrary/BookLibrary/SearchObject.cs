using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BookLibServices;
using System.Collections.ObjectModel;
using System.Reflection;

namespace BookLibrary
{
    class SearchObject
    {
        public string Name { get; set; }
        public string Author { get; set; }
        public DateTime Date { get; set; }
        public string Serial { get; set; }
        public string Type { get; set; }
        public string Subject { get; set; }
        public string Subtype { get; set; }
        public string Category { get; set; }
        public string Edition { get; set; }
        public ObservableCollection<string> Types { get; set; }

        public SearchObject (Dictionary<string, string> parameters, DateTime date)
    	{
            
            foreach (string item in parameters.Keys)
            {
                typeof(SearchObject).GetProperty(item).SetValue(this, parameters[item], null);
            }
            Date = date;
    	}
    }
}
