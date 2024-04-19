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
        public List<Feedback> Feedbacks { get; set; } = new List<Feedback>();

        public Athlete(int id, string firstName, string lastName, string streetName, string houseNumber, string postalcode, List<Workout> workouts, Location location, List<Feedback> feedbacks)
            : base(id, firstName, lastName, streetName, houseNumber, postalcode)
        {
            Workouts = workouts;
            Location = location;
            Feedbacks = feedbacks;
        }
        public Athlete(int id, string firstName, string lastName, string streetName, string houseNumber, string postalcode, Location location, List<Feedback> feedbacks)
            : base(id, firstName, lastName, streetName, houseNumber, postalcode)
        {
            Location = location;
            Feedbacks = feedbacks;
        }

        public Athlete(int id, string firstName, string lastName, string streetName, string houseNumber, string postalcode, Location location)
            : base(id, firstName, lastName, streetName, houseNumber, postalcode)
        {
            Location = location;
        }
    }
}
