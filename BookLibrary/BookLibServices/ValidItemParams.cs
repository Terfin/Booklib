using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BookLibServices
{
    /*
     * This is a list of valid item params that can be used to instantiate an AbstractItem based class.
     */
    public enum ValidItemParams
    {
        Name,
        Author,
        Location,
        EditionNumber,
        ISBN
    }
}
