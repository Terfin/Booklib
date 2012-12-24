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
    public static class SearchHelper
    {
        public delegate List<AbstractItem> SearchOptions(string param, List<AbstractItem> items);        
        private static ItemCollection collection = ItemCollection.Instance;

        public static List<AbstractItem> searchByName(string name, List<AbstractItem> origin = null)
        {
            if (origin == null)
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
            if (origin == null)
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
            if (origin == null)
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
            if (origin == null)
            {
                return collection[new ISBN(serial)];
            }
            else
            {
                return origin.FindAll(x => x.ISBN == new ISBN(serial));
            }
        }

        public static List<AbstractItem> searchByType(string type, List<AbstractItem> origin = null)
        {
            origin = origin == null ? collection.Items : origin;
            string fixedType = type.Replace(" ", string.Empty);
            Type itemType = Type.GetType(string.Format("BookLibServices.{0}, BookLibServices, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", fixedType));
            return origin.FindAll(x => x.GetType() == itemType);
        }

        public static List<AbstractItem> searchByCategory(string categoryName, List<AbstractItem> origin = null)
        {
            origin = origin == null ? collection.Items : origin;
            string fixedCategory = categoryName.Replace("&", string.Empty);
            fixedCategory = categoryName.Replace(" ", string.Empty);
                    postFiltered.AddRange(origin.FindAll(x =>
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
                    }));
                }
            }
            return postFiltered;
        }

        public static List<AbstractItem> searchByDate(string dateRange, List<AbstractItem> origin = null)
        {
            string[] rawDates = dateRange.Split('-');
            List<DateTime> dates = new List<DateTime>();
            CultureInfo culture = new CultureInfo("he-IL");
            List<AbstractItem> results = new List<AbstractItem>();
            foreach (string date in rawDates)
            {
                DateTime iterDate = DateTime.MinValue;
                string fixedDate = date.Replace(" ", string.Empty);
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
                for (int j = i * 2; j < dates.Count; i++)
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
            //if (dateStart != DateTime.MinValue || dateEnd != DateTime.MinValue)
            //{
            //    List<AbstractItem> postFiltered = new List<AbstractItem>();
            //    if (dateStart != DateTime.MinValue)
            //    {
            //        postFiltered.AddRange(origin.FindAll(x =>
            //            {
            //                return x.PrintDate >= dateStart;
            //            }));
            //    }
            //    if (dateEnd != DateTime.MinValue)
            //    {
            //        if (postFiltered.Count > 0)
            //        {
            //            origin = postFiltered;
            //        }
            //        postFiltered = origin.FindAll(x =>
            //            {
            //                return x.PrintDate <= dateEnd;
            //            });
            //    }
            //    return postFiltered;
            //}
            //else
            //{
            //    return origin;
            //}
        }
    }
}
