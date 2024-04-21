using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zuydfit
{
    internal class Administrator : Person
    {
        public List<Location> Locations { get; set; } = new List<Location>();
        public Feedback Feedback { get; set; }

        public Administrator(int id, string firstName, string lastName, string streetName, string houseNumber, string postalcode, List<Location> locations)
            : base(id, firstName, lastName, streetName, houseNumber, postalcode)
        {
            Locations = locations;
        }
    }
}