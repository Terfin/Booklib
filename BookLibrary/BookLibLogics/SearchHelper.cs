using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BookLibServices;
using BookLibDAL;
using System.Globalization;
using System.Reflection;

namespace BookLibLogics
{
    /*
     * Part 2 of the logics tier.
     * Well, this class provides all the needed search functions
     * It is static, for obvious reasons.
     */
    public static class SearchHelper
    {
        private static ItemCollection collection = ItemCollection.Instance; //Just getting the instance of the itemcollection so we can query it

        /*
         * All the methods in this class have the same structure, get a string parameter and an optional list of items and return a list of items
         * 
         */

        public static List<AbstractItem> searchByName(string name, List<AbstractItem> origin = null)
        {
            if (origin.Count == 0)
            {
                return collection[name];
            }
            else
            {
                return origin.FindAll(x => x.Name == name);
            }
        }

        public static List<AbstractItem> searchByAuthor(string author, List<AbstractItem> origin = null)
        {
            if (origin.Count == 0)
            {
                return collection.FindAll(x => x.Author == author);
            }
            else
            {
                return origin.FindAll(x => x.Author == author);
            }
        }

        public static List<AbstractItem> searchByEdition(string edition, List<AbstractItem> origin = null)
        {
            if (origin.Count == 0)
            {
                return collection.FindAll(x => x.Edition == int.Parse(edition));
            }
            else
            {
                return origin.FindAll(x => x.Edition == int.Parse(edition));
            }
        }

        public static List<AbstractItem> searchByISBN(string serial, List<AbstractItem> origin = null)
        {
            if (origin == null || origin.Count == 0 )
            {
                return collection[new ISBN(serial)];
            }
            else
            {
                return origin.FindAll(x => x.ISBN == new ISBN(serial));
            }
        }

        public static List<AbstractItem> searchByType(string type, List<AbstractItem> origin)
        {
            /*
             * We get the string representation of the type of object,
             * use reflection to get its actual type and then query the db
             * for any item that either matches that type or is a child of that type.
             */
            origin = origin == null || origin.Count == 0 ? collection.Items : origin;
            string fixedType = type.Replace(" ", string.Empty);
            Type itemType = Type.GetType(string.Format("BookLibServices.{0}, BookLibServices, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", fixedType));
            return origin.FindAll(x => x.GetType() == itemType || x.GetType().IsSubclassOf(itemType));
        }

        public static List<AbstractItem> searchByCategory(string categoryName, List<AbstractItem> origin = null)
        {
            /*
             * More or less like searching by type, only here, since we have a different set of categories
             * for every different type, we need to check the item's type first and then cast it into the right type.
             * All is done inside the lambda expression.
             */
            origin = origin == null || origin.Count == 0 ? collection : origin;
            string fixedCategory = categoryName.Replace("&", string.Empty);
            fixedCategory = fixedCategory.Replace(" ", string.Empty);
            return origin.FindAll(x =>
            {
                if (x.GetType() == typeof(ChildrenBook))
                {
                    ChildrenBook cb = x as ChildrenBook;
                    return cb.Category.ToString() == fixedCategory;
                }
                else if (x.GetType() == typeof(RegularBook))
                {
                    RegularBook rb = x as RegularBook;
                    return rb.Category.ToString() == fixedCategory;
                }
                else if (x.GetType() == typeof(StudyBook))
                {
                    StudyBook sb = x as StudyBook;
                    return sb.Category.ToString() == fixedCategory;
                }
                else if (x.GetType() == typeof(RegularJournal))
                {
                    RegularJournal rj = x as RegularJournal;
                    return rj.Category.ToString() == fixedCategory;
                }
                else if (x.GetType() == typeof(ScienceJournal))
                {
                    ScienceJournal sj = x as ScienceJournal;
                    return sj.Category.ToString() == fixedCategory;
                }
                else
                {
                    return false;
                }
            });
        }

        public static List<AbstractItem> searchByDate(string dateRange, List<AbstractItem> origin = null)
        {
            /*
             * Search by a date, either a single date or a range of dates.
             * Basically, this method is prepared to deal with inputs of number of ranges.
             * Date input must be formatted in the followin way: dd/MM/yyyy-dd/MM/yyyy-dd/MM/yyyy-dd/MM/yyyy... and so on.
             */
            string[] rawDates = dateRange.Split('-');
            List<DateTime> dates = new List<DateTime>();
            CultureInfo culture = new CultureInfo("he-IL");
            List<AbstractItem> results = new List<AbstractItem>();
            foreach (string date in rawDates)
            {
                DateTime iterDate = DateTime.MinValue;
                string fixedDate = date.Split(' ')[0];
                if (DateTime.TryParseExact(fixedDate, "dd/MM/yyyy", culture, DateTimeStyles.None, out iterDate))
                {
                    dates.Add(iterDate);
                }
                else
                {
                    throw new FormatException("Invalid date format, date must correspond to the following format 'dd/MM/yyyy'");
                }
            }
            origin = origin == null ? collection.Items : origin;
            for (int i = 0; i < (dates.Count % 2 == 0 ? dates.Count / 2 : (dates.Count / 2) + 1); i++)
            {
                List<AbstractItem> itermediateResults = new List<AbstractItem>();
                for (int j = i * 2; j < dates.Count; j++)
                {
                    if (j % 2 == 0)
                    {
                        itermediateResults.AddRange(origin.FindAll(x => x.PrintDate >= dates[j]));
                    }
                    else
                    {
                        results.AddRange(itermediateResults.FindAll(x => x.PrintDate <= dates[j]));
                    }
                }
            }
            return results;
        }
    }
}
