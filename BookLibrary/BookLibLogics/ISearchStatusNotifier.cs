using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BookLibServices;

namespace BookLibLogics
{
    public interface ISearchStatusNotifier
    {
        void searchComplete(List<AbstractItem> items);
    }
}
