using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BookLibServices
{
    public abstract class EAN
    {
        string _eanNumber;
        public EAN(string industryPrefix, string barcode)
        {
            int odd = 0;
            int even = 0;
            for (int i = 0; i < barcode.Length; i++)
            {
                int num = int.Parse(barcode[i].ToString());
                if ((i + 1) % 2 == 0)
                {
                    even += num;
                }
                else
                {
                    odd += num;
                }
            }
            int precheckdigit = (odd * 3 + even) % 10;
            int checkdigit = precheckdigit == 0 ? 0 : 10 - precheckdigit;
            _eanNumber = industryPrefix + barcode + checkdigit;
        }

        public string EANNumber 
        {
            get
            {
                return _eanNumber;
            }
        }
    }
}
