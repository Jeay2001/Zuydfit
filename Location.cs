using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zuydfit
{
    public class Location
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string StreetName { get; set; }
        public string HouseNumber { get; set; }
        public string PostalCode { get; set; }
        public List<Machine> Machines { get; set; }

        public Location(int id, string name, string streetName, string houseNumber, string postalCode, List<Machine> machines)
        {
            Id = id;
            Name = name;
            StreetName = streetName;
            HouseNumber = houseNumber;

            PostalCode = postalCode;
            Machines = machines;
        }
    }
}