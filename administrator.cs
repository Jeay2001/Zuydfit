using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zuydfit
{
    internal class Administrator : Person
    {

        public Administrator(int id, string firstName, string lastName, string streetName, string houseNumber, string postalcode)
            : base(id, firstName, lastName, streetName, houseNumber, postalcode)
        {

        }


    }
}
