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
        public int WorkoutId { get; set; }
        public int LocationId { get; set; }


        public Athlete(int id, string firstName, string lastName, string streetName, string houseNumber, string postalcode, List<Workout> workouts, Location location)
            : base(id, firstName, lastName, streetName, houseNumber, postalcode)
        {
            Workouts = workouts;
            Location = location;
        }
        public Athlete(int id, string firstName, string lastName, string streetName, string houseNumber, string postalcode, int workoutid, int locationid)
            : base(id, firstName, lastName, streetName, houseNumber, postalcode)
        {
            WorkoutId = workoutid;
            LocationId = locationid;
        }

        
    }
}
