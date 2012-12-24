using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BookLibServices
{
    public class ISSN : EAN
    {
        private static string industryPrefix = "977";
        public ISSN(string issnNumber, string edition) : base(industryPrefix, String.Concat(issnNumber.Take<char>(issnNumber.Length - 1)) + edition)
        {
            int n;
            if (issnNumber.Length != 8 || !int.TryParse(issnNumber, out n))
            {
                throw new InvalidSerialNumberException("ISSN number is invalid, must be consisted of only numbers and be at length 8");
            }
            else
            {
                Number = issnNumber;
                Edition = edition;
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

        public string Edition { get; private set; }
    }
}
