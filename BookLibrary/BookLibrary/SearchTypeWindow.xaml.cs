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
using System.Collections.ObjectModel;
using System.Reflection;
using BookLibServices;
using BookLibLogics;

namespace BookLibrary
{
    /// <summary>
    /// Interaction logic for SearchTypeWindow.xaml
    /// </summary>
    public partial class SearchTypeWindow : UserControl
    {
        private Utils utilities = Utils.Instance;
        private Dictionary<string, bool> checkedTypes = new Dictionary<string, bool>();
        private Dictionary<string, bool> checkedSubtypes = new Dictionary<string, bool>();
        private Dictionary<string, bool> checkedCategories = new Dictionary<string, bool>();
        private List<CheckBox> categoryBoxes = new List<CheckBox>();
        private ObservableCollection<string> subtypes = new ObservableCollection<string>();
        private ObservableCollection<CheckBox> categories1 = new ObservableCollection<CheckBox>();
        private ObservableCollection<CheckBox> categories2 = new ObservableCollection<CheckBox>();
        private ObservableCollection<CheckBox> categories3 = new ObservableCollection<CheckBox>();
        private ObservableCollection<CheckBox> categories4 = new ObservableCollection<CheckBox>();

        private string _isbn;
        private string _author;
        private string _edition;
        private string _subject;
        private string _dateFrom;
        private string _dateTo;

        public SearchTypeWindow()
        {
            InitializeComponent();
            itemSubTypeList.DataContext = subtypes;
            itemCategoriesList1.DataContext = categories1;
            itemCategoriesList2.DataContext = categories2;
            itemCategoriesList3.DataContext = categories3;
            itemCategoriesList4.DataContext = categories4;
            serialNumInp.DataContext = this;
            authorInp.DataContext = this;
            dateFromPicker.DataContext = this;
            dateToPicker.DataContext = this;
            itemEditionInp.DataContext = this;
        }

        public bool IsCheckBoxChecked
        {
            get
            {
                return (bool)GetValue(IsCheckBoxCheckedProperty);
            }
            set
            {
                SetValue(IsCheckBoxCheckedProperty, value);
            }
        }

        public static readonly DependencyProperty IsCheckBoxCheckedProperty =
            DependencyProperty.Register("IsCheckBoxChecked", typeof(bool), typeof(SearchTypeWindow), new UIPropertyMetadata(false));

        private void type_Checked(object sender, RoutedEventArgs e)
        {
            checkedTypes[((CheckBox)sender).Content.ToString()] = true;
            updateSubtypes();
        }

        private void type_Unchecked(object sender, RoutedEventArgs e)
        {
            string type = ((CheckBox)sender).Content.ToString();
            checkedTypes[type] = false;
            foreach (string subtype in utilities.getSubTypes(type))
            {
                if (CheckedSubtypes.ContainsKey(subtype))
                {
                    CheckedSubtypes[subtype] = false;
                }
            }
            updateSubtypes();
        }

        private void updateSubtypes()
        {
            subtypes.Clear();
            List<string> parames = new List<string>();
            if (checkedTypes.ContainsKey(itemTypeJournal.Content.ToString()) && checkedTypes[itemTypeJournal.Content.ToString()])
            {
                parames.Add("journal");
            }
            if (checkedTypes.ContainsKey(itemTypeBook.Content.ToString()) && checkedTypes[itemTypeBook.Content.ToString()])
            {
                parames.Add("book");
            }
            if (parames.Count > 0)
            {
                foreach (string subt in utilities.getSubTypes(parames))
                {
                    subtypes.Add(subt);
                }
                foreach (string box in itemSubTypeList.Items)
                {
                    if (checkedSubtypes.ContainsKey(box) && checkedSubtypes[box])
                    {
                        itemSubTypeList.SelectedItems.Add(box);
                    }
                }
            }
            updateCategories();
        }

        private void subType_Checked(object sender, RoutedEventArgs e)
        {
            checkedSubtypes[((CheckBox)sender).Content.ToString()] = true;
            updateCategories();
        }

        private void subType_Unchecked(object sender, RoutedEventArgs e)
        {
            string subType = ((CheckBox)sender).Content.ToString();
            
            if (subType.ToLower() != "{disconnecteditem}")
            {
                ValidateCategories(subType);
            }
            updateCategories();
        }
        
        private void ValidateCategories(string subType)
        {
            checkedSubtypes[subType] = false;
            foreach (string item in utilities.getCategories(subType))
            {
                if (checkedCategories.ContainsKey(item))
                {
                    CheckedCategories[item] = false;
                }
            }
        }


        private void updateCategories()
        {
            categories1.Clear();
            categories2.Clear();
            categories3.Clear();
            categories4.Clear();
            List<string> parames = new List<string>();
            foreach (string box in checkedSubtypes.Keys)
            {
                if (checkedSubtypes[box])
                {
                    parames.Add(box.ToLower());
                }
            }
            foreach (string category in utilities.getCategories(parames))
            {
                CheckBox box = new CheckBox();
                box.Content = category;
                box.FontSize = 16.0;
                box.Checked += category_Checked;
                box.Unchecked += category_Unchecked;
                if (CheckedCategories.ContainsKey(category))
                {
                    box.IsChecked = CheckedCategories[category];
                }
                if (categories1.Count < 8)
                {
                    categories1.Add(box);
                }
                else if (categories1.Count >= 8 && categories2.Count < 8)
                {
                    categories2.Add(box);
                }
                else if (categories2.Count >= 8 && categories3.Count < 8)
                {
                    categories3.Add(box);
                }
                else if (categories3.Count >= 8 && categories4.Count < 8)
                {
                    categories4.Add(box);
                }
            }
        }

        private void category_Checked(object sender, RoutedEventArgs e)
        {
            checkedCategories[((CheckBox)sender).Content.ToString()] = true;
        }

        private void category_Unchecked(object sender, RoutedEventArgs e)
        {
            checkedCategories[((CheckBox)sender).Content.ToString()] = false;
        }

        public Dictionary<string, bool> CheckedTypes
        {
            get
            {
                return checkedTypes;
            }
        }

        public Dictionary<string, bool> CheckedSubtypes
        {
            get
            {
                return checkedSubtypes;
            }
        }

        public Dictionary<string, bool> CheckedCategories
        {
            get
            {
                return checkedCategories;
            }
        }

        public string ISBN { get; set; }

        public string Author { get; set; }

        public string Edition { get; set; }

        public string DateFrom { get; set; }
        
        public string DateTo { get; set; }
        

    }
}
