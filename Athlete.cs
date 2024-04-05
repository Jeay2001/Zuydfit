using System;
using System.Collections.Generic;

namespace Zuydfit
{
    public class Athlete : Person
    {
        public List<Workout> Workouts { get; set; } = new List<Workout>();
        public Location? Location { get; set; } // Hier '?' toegevoegd om de eigenschap nullable te maken

        public Athlete(int id, string firstName, string lastName, string streetName, string houseNumber, string postalCode, List<Workout> workouts, Location? location) // Hier '?' toegevoegd om het type nullable te maken
            : base(id, firstName, lastName, streetName, houseNumber, postalCode)
        {
            Workouts = workouts;
            Location = location; // Toegewezen aan de eigenschap Location
        }

    }
}
