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

        public List<Exercise> Exercises { get; set; } = new List<Exercise>();
       


        public Athlete(int id, string firstName, string lastName, string streetName, string houseNumber, string postalcode, List<Exercise> exercises, List<Workout> workouts, Location location)
            : base(id, firstName, lastName, streetName, houseNumber, postalcode)
        {
            Exercises = exercises;
            Workouts = workouts;
            Location = location;

        }public Athlete(int id, string firstName, string lastName, string streetName, string houseNumber, string postalcode, List<Workout> workouts, Location location)
            : base(id, firstName, lastName, streetName, houseNumber, postalcode)
        {
            Workouts = workouts;
            Location = location;
        }
        public Athlete(int id, string firstName, string lastName, string streetName, string houseNumber, string postalcode, Location location)
            : base(id, firstName, lastName, streetName, houseNumber, postalcode)
        {
            Location = location;
        }
    }
}
