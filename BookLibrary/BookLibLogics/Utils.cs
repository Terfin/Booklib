using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using BookLibServices;

namespace BookLibrary
{
    public class Utils
    {
        private static Utils instance;
        private Dictionary<string, List<string>> categoryDict = new Dictionary<string, List<string>>();
        private List<string> journalTypes = new List<string>();
        private List<string> bookTypes = new List<string>();
        private Utils()
        {
            genCategories();
            genSubtypes();
        }

        public static Utils Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Utils();
                }
                return instance;
            }
        }

        private void genCategories()
        {
            Assembly amb = typeof(AbstractItem).Assembly;
            foreach (Type type in amb.GetTypes())
            {
                string typestr = type.Name.ToLower();
                if (typestr != "journal" && typestr != "book" && (typestr.Contains("journal") || typestr.Contains("book")))
                {
                    Type enumtype = type.GetNestedType("Categories");
                    FieldInfo[] fields = enumtype.GetFields();
                    List<string> catFields = new List<string>();
                    for (int i = 1; i < fields.Length; i++)
                    {
                        if (NameAttr.IsDefined(fields[i], typeof(NameAttr)))
                        {
                            NameAttr attr = (NameAttr)fields[i].GetCustomAttributes(false)[0];
                            catFields.Add(attr.Desc);
                        }
                        else
                        {
                            catFields.Add(fields[i].Name);
                        }
                    }
                    if (NameAttr.IsDefined(type, typeof(NameAttr)))
                    {
                        NameAttr attr = (NameAttr)type.GetCustomAttributes(false)[0];
                        categoryDict[attr.Desc.ToLower()] = catFields;
                    }
                    else
                    {
                        categoryDict[type.Name.ToLower()] = catFields;
                    }
                }
            }
        }

        private void genSubtypes()
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

        public List<string> getCategories(List<string> subTypes)
        {
            List<string> categories = new List<string>();
            foreach (string type in subTypes)
            {
                categories.AddRange(categoryDict[type].Where(x => !categories.Contains(x)));
            }
            return categories;
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
