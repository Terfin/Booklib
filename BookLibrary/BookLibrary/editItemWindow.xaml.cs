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
        private string selectedType;
        private string selectedSubtype;
        private string selectedCategory;
        public editTypes etype;
        AbstractItem editedItem;
        ObservableCollection<string> _subTypes = new ObservableCollection<string>();
        public event EventHandler editActionCompleted;
        private bool _bestseller;

        public enum editTypes
        {
            Add, Edit
        }

        public editItemWindow(editTypes edType, AbstractItem item = null)
        {
            InitializeComponent();
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
            bestsellerInp.DataContext = this;
            itemSubjectInp.DataContext = this;
            etype = edType;
            if (edType == editTypes.Add)
            {
                saveButton.Visibility = System.Windows.Visibility.Hidden;
                addButton.Visibility = System.Windows.Visibility.Visible;
                serialNumInp.IsEnabled = false;
            }
            else if (edType == editTypes.Edit)                                                  
            {
                editedItem = item;
                editItem(item);
            }
        }

        private void editItem(AbstractItem item)
        {
            addButton.Visibility = System.Windows.Visibility.Hidden;
            saveButton.Visibility = System.Windows.Visibility.Visible;
            SelectedType = item.GetType().IsSubclassOf(typeof(Book)) ? "Book" : "Journal";
            SelectedSubtype = ((NameAttr)item.GetType().GetCustomAttributes(typeof(NameAttr), false)[0]).Desc;
            if (item.GetType().IsSubclassOf(typeof(Book)))
            {
                if (item is RegularBook)
                {
                    RegularBook book = item as RegularBook;
                    object[] refCat = typeof(RegularBook.Categories).GetField(book.Category.ToString()).GetCustomAttributes(typeof(NameAttr), false);
                    SelectedCategory = refCat.Length > 0 ? ((NameAttr)refCat[0]).Desc : book.Category.ToString();
                    this.DataContext = book;
                }
                else if (item is ChildrenBook)
                {
                    ChildrenBook book = item as ChildrenBook;
                    object[] refCat = typeof(ChildrenBook.Categories).GetField(book.Category.ToString()).GetCustomAttributes(typeof(NameAttr), false);
                    SelectedCategory = refCat.Length > 0 ? ((NameAttr)refCat[0]).Desc : book.Category.ToString();
                    this.DataContext = book;
                }
                else if (item is StudyBook)
                {
                    StudyBook book = item as StudyBook;
                    object[] refCat = typeof(StudyBook.Categories).GetField(book.Category.ToString()).GetCustomAttributes(typeof(NameAttr), false);
                    SelectedCategory = refCat.Length > 0 ? ((NameAttr)refCat[0]).Desc : book.Category.ToString();
                    this.DataContext = book;
                }
                IsBestseller = ((Book)item).IsBestseller;
            }
            else if (item.GetType().IsSubclassOf(typeof(Journal)))
            {
                if (item is RegularJournal)
                {
                    RegularJournal journal = item as RegularJournal;
                    object[] refCat = typeof(RegularJournal.Categories).GetField(journal.Category.ToString()).GetCustomAttributes(typeof(NameAttr), false);
                    SelectedCategory = refCat.Length > 0 ? ((NameAttr)refCat[0]).Desc : journal.Category.ToString();
                    this.DataContext = journal;
                }
                else if (item is ScienceJournal)
                {
                    ScienceJournal journal = item as ScienceJournal;
                    object[] refCat = typeof(ScienceJournal.Categories).GetField(journal.Category.ToString()).GetCustomAttributes(typeof(NameAttr), false);
                    SelectedCategory = refCat.Length > 0 ? ((NameAttr)refCat[0]).Desc : journal.Category.ToString();
                    this.DataContext = journal;
                }
                Subject = ((Journal)item).Subject;
            }
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
                if (value == "Book")
                {
                    itemSubjectInp.IsEnabled = false;
                    bestsellerInp.IsEnabled = true;
                }
                else if (value == "Journal")
                {
                    itemSubjectInp.IsEnabled = true;
                    bestsellerInp.IsEnabled = false;
                }
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
        public string Subject { get; set; }
        public bool IsBestseller
        {
            get
            { return _bestseller; }
            set
            { _bestseller = value; }
        }
        public ObservableCollection<string> Subtypes
        {
            get { return _subTypes; }
        }
        public ObservableCollection<string> CategoriesList { get; private set; }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (SelectedSubtype != ((NameAttr)editedItem.GetType().GetCustomAttributes(typeof(NameAttr), false)[0]).Desc)
                {
                    data.RemoveItem(editedItem);
                    createNewItem();
                }
                else
                {
                    editExistingItem();
                }
            }
            catch (InvalidCastException error)
            {
                MessageBox.Show(error.Message);
            }
        }

        private void createNewItem()
        {
            if (validateMandatoryInputs())
            {
                Dictionary<ValidItemParams, string> newItemParams = new Dictionary<ValidItemParams, string>()
            {
                { ValidItemParams.Name, nameField.Text },
                { ValidItemParams.Author, authorInp.Text },
                { ValidItemParams.Location, locBox.Text }
            };
                if (serialNumInp.Text.Length > 0)
                {
                    newItemParams[ValidItemParams.ISBN] = serialNumInp.Text;
                }
                if (itemEditionInp.Text.Length > 0)
                {
                    newItemParams[ValidItemParams.EditionNumber] = itemEditionInp.Text;
                }
                string realType = selectedSubtype.Replace(" ", string.Empty);
                Type itemType = Type.GetType(string.Format("BookLibServices.{0}, BookLibServices, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", realType));
                string refinedCategory = SelectedCategory.Replace("&", string.Empty);
                refinedCategory = refinedCategory.Replace(" ", string.Empty);
                AbstractItem item = null;
                if (itemType == typeof(RegularBook))
                {
                    item = new RegularBook(newItemParams, (DateTime)dateFromPicker.SelectedDate, (RegularBook.Categories)Enum.Parse(typeof(RegularBook.Categories), refinedCategory), IsBestseller);
                }
                else if (itemType == typeof(ChildrenBook))
                {
                    item = new ChildrenBook(newItemParams, (DateTime)dateFromPicker.SelectedDate, (ChildrenBook.Categories)Enum.Parse(typeof(ChildrenBook.Categories), refinedCategory), IsBestseller);
                }
                else if (itemType == typeof(StudyBook))
                {
                    item = new StudyBook(newItemParams, (DateTime)dateFromPicker.SelectedDate, (StudyBook.Categories)Enum.Parse(typeof(StudyBook.Categories), refinedCategory), IsBestseller);
                }
                else if (itemType == typeof(RegularJournal))
                {
                    item = new RegularJournal(newItemParams, (DateTime)dateFromPicker.SelectedDate, Subject, (RegularJournal.Categories)Enum.Parse(typeof(RegularJournal.Categories), refinedCategory));
                }
                else if (itemType == typeof(ScienceJournal))
                {
                    item = new ScienceJournal(newItemParams, (DateTime)dateFromPicker.SelectedDate, Subject, (ScienceJournal.Categories)Enum.Parse(typeof(ScienceJournal.Categories), refinedCategory));
                }
                data.AddItem(item);
                editActionCompleted(this, null);
            }
        }

        private bool validateMandatoryInputs()
        {
            if (nameField.Text.Length == 0)
            {
                MessageBox.Show("Name cannot be left blank, please enter a name");
                return false;
            }
            else if (authorInp.Text.Length == 0)
            {
                MessageBox.Show("Author cannot be left blank, please enter the item's author's name!");
                return false;
            }
            else if (dateFromPicker.SelectedDate == null)
            {
                MessageBox.Show("Please choose the item's publication date!");
                return false;
            }
            else if (SelectedType == null)
            {
                MessageBox.Show("Please choose the item's type!");
                return false;
            }
            else if (SelectedSubtype == null)
            {
                MessageBox.Show("Please select the item's subtype!");
                return false;
            }
            else if (SelectedCategory == null)
            {
                MessageBox.Show("Please select the item's category!");
                return false;
            }
            return true;
        }

        private void editExistingItem()
        {
            if (validateMandatoryInputs())
            {
                int editionNumber;
                if (int.TryParse(itemEditionInp.Text, out editionNumber))
                {
                    editedItem.Edition = editionNumber;
                }
                else
                {
                    throw new InvalidCastException("Edition must be a number only!");
                }
                string refinedCategory = SelectedCategory.Replace("&", string.Empty);
                refinedCategory = refinedCategory.Replace(" ", string.Empty);
                editedItem.Name = nameField.Text;
                editedItem.ISBN = new ISBN(serialNumInp.Text);
                editedItem.Location = locBox.Text;
                editedItem.PrintDate = (DateTime)dateFromPicker.SelectedDate;
                editedItem.Author = authorInp.Text;
                if (editedItem.GetType().IsSubclassOf(typeof(Book)))
                {
                    if (editedItem is RegularBook)
                    {
                        RegularBook book = editedItem as RegularBook;
                        book.Category = (RegularBook.Categories)Enum.Parse(typeof(RegularBook.Categories), refinedCategory);
                    }
                    else if (editedItem is ChildrenBook)
                    {
                        ChildrenBook book = editedItem as ChildrenBook;
                        book.Category = (ChildrenBook.Categories)Enum.Parse(typeof(ChildrenBook.Categories), refinedCategory);
                    }
                    else if (editedItem is StudyBook)
                    {
                        StudyBook book = editedItem as StudyBook;
                        book.Category = (StudyBook.Categories)Enum.Parse(typeof(StudyBook.Categories), refinedCategory);
                    }
                    ((Book)editedItem).IsBestseller = IsBestseller;
                }
                else if (editedItem.GetType().IsSubclassOf(typeof(Journal)))
                {
                    if (editedItem is RegularJournal)
                    {
                        RegularJournal journal = editedItem as RegularJournal;
                        journal.Category = (RegularJournal.Categories)Enum.Parse(typeof(RegularJournal.Categories), refinedCategory);
                    }
                    else if (editedItem is ScienceJournal)
                    {
                        ScienceJournal journal = editedItem as ScienceJournal;
                        journal.Category = (ScienceJournal.Categories)Enum.Parse(typeof(ScienceJournal.Categories), refinedCategory);
                    }
                    ((Journal)editedItem).Subject = Subject;
                }
                editActionCompleted(this, null);
            }
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                createNewItem();
            }
            catch (InvalidParameterException error)
            {
                MessageBox.Show(error.Message);
            }
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            editActionCompleted(this, null);
        }
    }
}
