using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using BookLibServices;


namespace BookLibrary
{
    internal class Categories
    {
        Dictionary<string, List<string>> categoryDict = new Dictionary<string, List<string>>();
        private static Categories instance;

        private Categories()
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

        public static Categories Instance
        {
            get
            {
                 if (instance == null)
                {
                    instance = new Categories();
                }
                return instance;
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
    }
}
