using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zuydfit
{
    public class Workout
    {
        public int Id { get; set; }
        public DateOnly Date { get; set; }
        public List<Exercise> Exercises { get; set; }

        public Workout(int id, DateOnly date)
        {
            Id = id;
            Date = date;
            Exercises = new List<Exercise>();
        }

        public Workout(int id, DateOnly date, List<Exercise> exercises)
        {
            Id = id;
            Date = date;
            Exercises = exercises;
        }


    }
}
