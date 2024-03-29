﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BookLibServices
{
    /*
     * This class represents a unique serial number which can belong only to one book,
     * but can belong to as many copies as there are of that book.
     */
    public class ISBN
    {
        private delegate string ISBNGenerator(string number);

        private static string lastNumber;
        
        public ISBN()
        {
            ISBNGenerator generator = NewItemISBN;
            this.Number = GenerateNewISBN(generator);
        }

        public ISBN(string number)
        {
            ISBNGenerator generator = GenerateISBNFromString;
            this.Number = GenerateValidISBN(number, generator);
        }

        private string GenerateNewISBN(ISBNGenerator generator)
        {
            string nextNumber = "";
            if (lastNumber != null)
            {
                double LastISBN = ParseISBNString(lastNumber);
                nextNumber = GenerateValidISBN((LastISBN + 1).ToString(), generator);
            }
            else
            {
                nextNumber = "000-000-001";
            }
            lastNumber = nextNumber;
            return nextNumber;
        }

        private double ParseISBNString(string isbn)
        {
            isbn = isbn.Replace("-", string.Empty);
            return int.Parse(isbn);
        }

        private string GenerateValidISBN(string isbn, ISBNGenerator generator)
        {
            return generator(isbn);
        }

        private static string NewItemISBN(string isbn)
        {
            string isbnString = "";
            for (int i = 9; i >= 1; i--)
            {
                if (i % 3 == 0 && i < 9 && i > 0)
                {
                    isbnString += "-";
                }
                if (i > isbn.Length)
                {
                    isbnString += "0";
                }
                else
                {
                    isbnString += isbn[i - 1];
                }
            }
            return isbnString;
        }

        private string GenerateISBNFromString(string isbn)
        {
            string isbnString = "";
            while (isbn.Contains('-'))
            {
                isbn = isbn.Replace("-", string.Empty);
            }
            for (int i = 1; i <= isbn.Length; i++)
            {
                isbnString += isbn[i - 1];
                if (i % 3 == 0 && i < 9)
                {
                    isbnString += "-";
                }
            }
            return isbnString;
        }

        public override bool Equals(object obj)
        {
            return ((ISBN)obj).Number == this.Number;
        }

        public override int GetHashCode()
        {
            string hashcode = Number.Replace("-", string.Empty);
            return int.Parse(hashcode);
        }

        public string Number { get; private set; }
    }
}
