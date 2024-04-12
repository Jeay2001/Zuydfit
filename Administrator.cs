using System.Collections.Generic;

namespace Zuydfit
{
    internal class Administrator : Person
    {
        // Lijst van coaches die door de administrator worden beheerd
        public List<Coach> Coaches { get; set; } = new List<Coach>();
        public List<Location> Locations { get; set; } = new List<Location>();


        // Constructor
        public Administrator(int id, string firstName, string lastName, string streetName, string houseNumber, string postalcode, List<Location> locations, List<Coach> coaches)
            : base(id, firstName, lastName, streetName, houseNumber, postalcode)
        {
            Coaches = coaches;
            Locations = locations;       
        }
    }
}