using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BookLibServices
{
    class Librarian : User
    {
        public Librarian(string name, Type specialty, int hourlySalary, string password)
            :base(name)
        {
            this.Specialty = specialty;
            this.HourlySalary = hourlySalary;
            this.Password = password;
        }

        public Type Specialty { get; private set; }
        public int HourlySalary { get; private set; }
        public string Password { get; private set; }
    }
}
