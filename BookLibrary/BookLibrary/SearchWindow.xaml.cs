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
using BookLibLogics;
using BookLibServices;

namespace BookLibrary
{
    /// <summary>
    /// Interaction logic for SearchWindow.xaml
    /// </summary>
    public partial class SearchWindow : UserControl
    {
        SearchTypeWindow stype;
        SearchResultsWindow sresults;
        ISNotify inotify;
        DynamicData dyndata;

        public delegate List<AbstractItem> SearchFunction<T>(T parameter, List<AbstractItem> items);

        public SearchWindow()
        {
            InitializeComponent();
            stype = new SearchTypeWindow();
            inotify = new ISNotify(this);
            dyndata = DynamicData.Instance;
            dyndata.SearchWindowNotifier = inotify;
            sresults = new SearchResultsWindow();
            expanderContent.Content = sresults;
            sresults.EditActionInvoked += new EventHandler(sresults_EditActionInvoked);
            List<SearchFunction<string>> funcset1 = new List<SearchFunction<string>>();
            List<SearchFunction<DateTime>> funcset2 = new List<SearchFunction<DateTime>>();
        }

        void sresults_EditActionInvoked(object sender, EventArgs e)
        {
            expanderContent.Content = new editItemWindow(editItemWindow.editTypes.Edit, (AbstractItem)sender);
        }

        private void OptionsExpander_Expanded(object sender, RoutedEventArgs e)
        {
            expanderContent.Content = stype;
        }

        private void OptionsExpander_Collapsed(object sender, RoutedEventArgs e)
        {
            expanderContent.Content = sresults;
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            List<AbstractItem> results = null;
            
            if (optionsExpander.IsExpanded)
            {
                foreach (ValidSearchParams param in stype.activeFields.Keys)
                {
                    
                }
            }
            dyndata.Search(parameters);
        }
    }
    
    public class ISNotify : ISearchStatusNotifier
    {
        private SearchWindow activeWindow;
        public ISNotify(SearchWindow window)
        {
            activeWindow = window;
        }
        public void searchComplete(List<AbstractItem> items)
        {
            activeWindow.optionsExpander.IsExpanded = false;
        }
    }
}
