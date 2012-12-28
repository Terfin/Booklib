using System;
using System.Collections.ObjectModel;
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
using System.Threading;
using BookLibServices;
using BookLibLogics;
using System.Data;
using System.Globalization;

namespace BookLibrary
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
   
    public partial class MainWindow : Window
    {
        /* Nothing important here, except for setting the culture in order to get a normal date format.
         * Basically, if I realized how to implement MVVM (or MVC) at a early stage and not at the end of the project
         * I would create all the user control instances here and make this class as a master controller.
         * Unfortunately, I didn't do it and remaking the entire project will take too long.
         */
        SearchWindow swin = new SearchWindow();
        public MainWindow()
        {
            CultureInfo ci = CultureInfo.CreateSpecificCulture(CultureInfo.CurrentCulture.Name);
            ci.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
            Thread.CurrentThread.CurrentCulture = ci;
            DynamicData foo = DynamicData.Instance;
            InitializeComponent();
            mainContent.Content = swin;
            
        }
    }
}
