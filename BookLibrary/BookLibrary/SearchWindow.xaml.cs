﻿using System;
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
        DynamicData dyndata;


        public SearchWindow()
        {
            InitializeComponent();
            stype = new SearchTypeWindow();
            dyndata = DynamicData.Instance;
            sresults = new SearchResultsWindow();
            expanderContent.Content = sresults;
            sresults.EditActionInvoked += sresults_EditActionInvoked;
            dyndata.onSearchComplete += searchComplete;
        }

        void sresults_EditActionInvoked(object sender, EventArgs e)
        {
            editItemWindow editWindow = new editItemWindow(editItemWindow.editTypes.Edit, (AbstractItem)sender);
            editWindow.editActionCompleted += editDone;
            expanderContent.Content = editWindow;
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
            List<DynamicData.SearchFunction> searchFuncSet = new List<DynamicData.SearchFunction>();
            List<List<string>> listOfValuesLists = new List<List<string>>();
            if (itemNameInp.Text.Length > 0)
            {
                listOfValuesLists.Add(new List<string>() { itemNameInp.Text });
                searchFuncSet.Add(SearchHelper.searchByName);
            }
            if (optionsExpander.IsExpanded)
            {
                if (stype.serialNumInp.Text.Length > 0)
                {
                    listOfValuesLists.Add(new List<string>() { stype.serialNumInp.Text });
                    searchFuncSet.Add(SearchHelper.searchByISBN);
                }
                if (stype.itemEditionInp.Text.Length > 0)
                {
                    listOfValuesLists.Add(new List<string>() { stype.itemEditionInp.Text });
                    searchFuncSet.Add(SearchHelper.searchByEdition);
                }
                if (stype.authorInp.Text.Length > 0)
                {
                    listOfValuesLists.Add(new List<string>() { stype.authorInp.Text });
                    searchFuncSet.Add(SearchHelper.searchByAuthor);
                }
                string dateString = "";
                if (stype.dateFromPicker.SelectedDate != null)
                {
                    dateString += ((DateTime)stype.dateFromPicker.SelectedDate).Date;
                }
                if (stype.dateToPicker.SelectedDate != null)
                {
                    dateString += "-" + ((DateTime)stype.dateToPicker.SelectedDate).Date;
                }
                if (dateString.Length > 0)
                {
                    listOfValuesLists.Add(new List<string>() { dateString });
                    searchFuncSet.Add(SearchHelper.searchByDate);
                }
                Dictionary<string, bool> checkedTypes = stype.CheckedTypes.Where(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
                Dictionary<string, bool> checkedSubtypes = stype.CheckedSubtypes.Where(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
                if (checkedTypes.Keys.Count > 0)
                {
                    listOfValuesLists.Add(checkedTypes.Keys.ToList());
                    searchFuncSet.Add(SearchHelper.searchByType);
                }
                if (checkedSubtypes.Keys.Count > 0)
                {
                    listOfValuesLists.Add(checkedSubtypes.Keys.ToList());
                    searchFuncSet.Add(SearchHelper.searchByType);
                }
                Dictionary<string, bool> checkedCategories = stype.CheckedCategories.Where(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
                if (checkedCategories.Keys.Count > 0)
                {
                    listOfValuesLists.Add(checkedCategories.Keys.ToList());
                    searchFuncSet.Add(SearchHelper.searchByCategory);
                }
                if (searchFuncSet.Count > 0)
                {
                    dyndata.Search(listOfValuesLists, searchFuncSet);
                }
                else
                {
                    dyndata.Search();
                }
            }
            else
            {
                dyndata.Search();
            }
        }

        public void searchComplete(List<AbstractItem> items)
        {
            optionsExpander.IsExpanded = false;
        }

        public void editDone(object sender, EventArgs e)
        {
            editItemWindow editedItemWindow = (editItemWindow)sender;
            if (editedItemWindow.serialNumInp.Text.Length > 0)
            {
                dyndata.Search(new List<List<string>>()
                {
                    new List<string>()
                    {
                        editedItemWindow.serialNumInp.Text
                    }
                }, new List<DynamicData.SearchFunction>()
                {
                    SearchHelper.searchByISBN
                });
            }
            else
            {
                dyndata.Search();
            }
            optionsExpander.IsExpanded = true;
            optionsExpander.IsExpanded = false;
            editedItemWindow.editActionCompleted -= editDone;
        }

        private void addItemButton_Click(object sender, RoutedEventArgs e)
        {
            optionsExpander.IsExpanded = false;
            editItemWindow newItemWindow = new editItemWindow(editItemWindow.editTypes.Add);
            newItemWindow.editActionCompleted += editDone;
            expanderContent.Content = newItemWindow;
        }
    }
}
