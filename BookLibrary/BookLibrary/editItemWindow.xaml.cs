using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BookLibServices;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Reflection;
using BookLibLogics;
using Microsoft.CSharp;
using System.IO;

namespace BookLibrary
{
    /// <summary>
    /// Interaction logic for editItemWindow.xaml
    /// </summary>
    public partial class editItemWindow : UserControl
    {
        private Utils utilities = Utils.Instance;
        private DynamicData data = DynamicData.Instance;
        Dictionary<string, string> parameters;
        AbstractItem editedItem;
        SearchObject sobj;
        DateTime editedItemPublishDate;
        private string selectedType;
        private string selectedSubtype;
        private string selectedCategory;
        ObservableCollection<string> _subTypes = new ObservableCollection<string>();

        public enum editTypes
        {
            Add, Edit
        }

        public editItemWindow(editTypes edType, AbstractItem item = null)
        {
            InitializeComponent();
            editedItem = item;
            List<Type> tlist = typeof(Book).Assembly.GetTypes().ToList<Type>();
            List<string> Types = new List<string>()
                {
                    "Book", "Journal"
                };
            CategoriesList = new ObservableCollection<string>();
            typeCombo.ItemsSource = Types;
            typeCombo.DataContext = this;
            subTypeCombo.DataContext = this;
            categoryCombo.DataContext = this;
            if (edType == editTypes.Add)
            {
                saveButton.Visibility = System.Windows.Visibility.Hidden;
                addButton.Visibility = System.Windows.Visibility.Visible;
            }
            else if (edType == editTypes.Edit)
            {
                addButton.Visibility = System.Windows.Visibility.Hidden;
                saveButton.Visibility = System.Windows.Visibility.Visible;
                sobj = getSearchObject(item);
                SelectedType = sobj.Type;
                SelectedSubtype = sobj.Subtype;
                SelectedCategory = sobj.Category;
                this.DataContext = sobj;
            }
        }

        private SearchObject getSearchObject(AbstractItem item)
        {
            FieldInfo fi = null;
            parameters = null;
            DateTime itemPrintDate = DateTime.MinValue;
            if (item is ChildrenBook)
            {
                ChildrenBook book = item as ChildrenBook;
                parameters = getParamsByBook(book);
                fi = book.Category.GetType().GetField(book.Category.ToString());
                itemPrintDate = book.PrintDate;
                this.DataContext = book;
            }
            else if (item is RegularBook)
            {
                RegularBook book = item as RegularBook;
                parameters = getParamsByBook(book);
                fi = book.Category.GetType().GetField(book.Category.ToString());
            }
            else if (item is StudyBook)
            {
                StudyBook book = item as StudyBook;
                parameters = getParamsByBook(book);
                fi = book.Category.GetType().GetField(book.Category.ToString());
            }
            else if (item is RegularJournal)
            {
                RegularJournal journal = item as RegularJournal;
                parameters = getParamsByJournal(journal);
                fi = journal.Category.GetType().GetField(journal.Category.ToString());
            }
            else if (item is ScienceJournal)
            {
                ScienceJournal journal = item as ScienceJournal;
                parameters = getParamsByJournal(journal);
                fi = journal.Category.GetType().GetField(journal.Category.ToString());
            }
            parameters["Category"] = ((NameAttr)fi.GetCustomAttributes(typeof(NameAttr), false)[0]).Desc;
            SearchObject sobj = new SearchObject(parameters, itemPrintDate);
            return sobj;
        }

        private Dictionary<string, string> getParamsByJournal(Journal journal)
        {
            Dictionary<string, string> parameters = getParams(journal);
            parameters["Type"] = "Journal";
            parameters["Subject"] = journal.Subject;
            return parameters;
        }

        private static Dictionary<string, string> getParamsByBook(Book book)
        {
            Dictionary<string, string> parameters = getParams(book);
            parameters["Type"] = "Book";
            return parameters;
        }

        private static Dictionary<string, string> getParams(AbstractItem item)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters["Name"] = item.Name;
            parameters["Author"] = item.Author;
            parameters["Subtype"] = ((NameAttr)item.GetType().GetCustomAttributes(typeof(NameAttr), false)[0]).Desc;
            parameters["Author"] = item.Author;
            parameters["ISBN"] = item.ISBN.Number;
            parameters["Edition"] = item.Edition.ToString();
            parameters["Location"] = item.Location;
            return parameters;
        }

        public string SelectedType 
        { 
            get
            {
                return selectedType;
            }
            set
            {
                selectedType = value;
                List<string> subtypes = utilities.getSubTypes(new List<string>()
                {
                    value.ToLower()
                });
                _subTypes.Clear();
                foreach (string subtype in subtypes)
                {
                    _subTypes.Add(subtype);
                }
            }
        }

        public string SelectedSubtype
        {
            get
            {
                return selectedSubtype;
            }
            set
            {
                if (value != null)
                {
                    CategoriesList.Clear();
                    selectedSubtype = value;
                    List<string> categories = utilities.getCategories(new List<string>()
                {
                    value.ToLower()
                });
                    foreach (string category in categories)
                    {
                        CategoriesList.Add(category);
                    }
                }
                else
                {
                    CategoriesList.Clear();
                }
            }
        }

        public string SelectedCategory { get; set; }
        public string Edition { get; private set; }
        public ObservableCollection<string> Subtypes
        {
            get { return _subTypes; }
        }
        public ObservableCollection<string> CategoriesList { get; private set; }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            CSharpCodeProvider provider = new CSharpCodeProvider();
            
            AbstractItem item;
            bool newItem = false;
            if (SelectedSubtype != ((NameAttr)editedItem.GetType().GetCustomAttributes(typeof(NameAttr), false)[0]).Desc)
            {
                data.RemoveItem(editedItem);
                newItem = true;
            }
            if (sobj.Name != editedItem.Name)
            {
            }
            
        }

        private AbstractItem createNewItem()
        {
            string codeToRun = "";
            codeToRun += getNewInstanceString(codeToRun);
        }


        private string getNewInstanceString(string codeToRun)
        {
            string realTypestr = selectedSubtype.Replace("&", string.Empty);
            realTypestr = realTypestr.Replace(" ", string.Empty);
            Type itemType = Type.GetType(string.Format("BookLibServices.{0}, BookLibServices, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", realTypestr));
            if (itemType == typeof(RegularBook))
            {
                return "new RegularBook(";
            }
            else if (itemType == typeof(ChildrenBook))
            {
                return "new ChildrenBook(";
            }
            else if (itemType == typeof(StudyBook))
            {
                return "new StudyBook(";
            }
            else if (itemType == typeof(RegularJournal))
            {
                return "new RegularJournal(";
            }
            else if (itemType == typeof(ScienceJournal))
            {
                return "new ScienceJournal(";
            }
            return "";
        }
    }
}
