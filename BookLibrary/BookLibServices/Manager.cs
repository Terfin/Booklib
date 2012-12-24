using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BookLibServices
{
    class Manager : Librarian
    {
        public Manager(string name, Type specialty, int hourlySalary, string password)
            :base(name, specialty, hourlySalary, password)
        {
        }
    }
}
