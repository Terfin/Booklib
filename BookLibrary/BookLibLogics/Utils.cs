using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using BookLibServices;
using System.Windows.Controls;

namespace BookLibrary
{
    /*
     * Part 3 of BookLib Logics.
     * This class contains several utility functions,
     * This class could be a static class instead of a singleton,
     * I did it as singleton mainly to memorize the pattern
     */
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

        private void genCategories() // Generates a category dictionary that can be easily queried to assist with UI functions.
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

        private void genSubtypes() //This method generates two lists of types, one for Book based types and one for Journal based types.
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

        public List<string> getCategories(List<string> subTypes) //Returns a list of categories for a given list of subtypes.... that's it.
        {
            List<string> categories = new List<string>();
            foreach (string type in subTypes)
            {
                categories.AddRange(categoryDict[type].Where(x => !categories.Contains(x)));
            }
            return categories;
        }

        public List<string> getCategories(string subType)
        {
            return categoryDict[subType.ToLower()];
        }

        public List<string> getSubTypes(List<string> types) //Returns a list of types...
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

        public List<string> getSubTypes(string type)
        {
            return getSubTypes(new List<string>() { type.ToLower() });
        }
    }
}
