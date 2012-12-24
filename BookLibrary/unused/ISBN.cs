using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BookLibServices
{
    public class ISBN : EAN
    {
        private static string industryPrefix = "978";
        public ISBN(string isbn) : base(industryPrefix, isbn)
        {
            int n;
            bool isValidISBN = int.TryParse(isbn, out n);
            if (isbn.Length != 9 || !isValidISBN)
            {
                throw new InvalidSerialNumberException("ISBN number invalid! IBSN must be a numeric value of length 9!");
            }
            else
            {
                string numWithoutDigit = isbn;
                int checkdigit = 0;
                for (int i = 8; i >= 1; i--)
                {
                    checkdigit += int.Parse(numWithoutDigit[i].ToString()) * (i + 1);
                }
                checkdigit = 11 - (checkdigit % 11);
                this.Number = isbn + checkdigit;
            }
        }

        public string Number { get; private set; }

        public static string IndustryPrefix
        {
            get
            {
                return industryPrefix;
            }
        }
    }
}
