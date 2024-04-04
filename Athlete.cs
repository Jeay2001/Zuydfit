using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zuydfit
{
    public class Athlete : Person
    {
        public List<Workout> Workouts { get; set; } = new List<Workout>(); 
        public Location Location { get; set; } 

        public Athlete(int id, string firstName, string lastName, string streetName, string houseNumber, string postalCode, List<Workout> workouts, Location Location)
            : base(id, firstName, lastName, streetName, houseNumber, postalCode)
        {
            Workouts = workouts;
            Location = Location;
        }

    }
}
