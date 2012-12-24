using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using BookLibServices;

namespace BookLibrary
{
    internal class SubTypes
    {
        private static SubTypes instance;
        private List<string> journalTypes = new List<string>();
        private List<string> bookTypes = new List<string>();
        private SubTypes()
        {
            Assembly assemTypes = typeof(AbstractItem).Assembly;
            foreach (Type type in assemTypes.GetTypes())
            {
                string typeStr = type.Name.ToLower();
                if (typeStr.Contains("journal") && typeStr != "journal")
                {
                    if (NameAttr.IsDefined(type, typeof(NameAttr)))
                    {
                        NameAttr attr = (NameAttr)type.GetCustomAttributes(false)[0];
                        journalTypes.Add(attr.Desc);
                    }
                    else
                    {
                        journalTypes.Add(type.Name);
                    }
                }
                else if (typeStr.Contains("book") && typeStr != "book")
                {
                    if (NameAttr.IsDefined(type, typeof(NameAttr)))
                    {
                        NameAttr attr = (NameAttr)type.GetCustomAttributes(false)[0];
                        bookTypes.Add(attr.Desc);
                    }
                    else
                    {
                        bookTypes.Add(type.Name);
                    }
                }
            }
        }

        public static SubTypes Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SubTypes();
                }
                return instance;
            }
        }

        public List<string> getSubTypes(List<string> types)
        {
            List<string> matchSubtypes = new List<string>();
            if (types.Contains("journal"))
            {
                matchSubtypes.AddRange(journalTypes);
            }
            if (types.Contains("book"))
            {
                matchSubtypes.AddRange(bookTypes);
            }
            return matchSubtypes;
        }
    }
}
