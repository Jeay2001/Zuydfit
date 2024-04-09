using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zuydfit.DataAccessLayer;

namespace Zuydfit
{
    public abstract class Exercise
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public Exercise(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public static List<Exercise> ReadExerciseListFromAthlete(Athlete athlete)
        {
            DAL dal = new DAL();
            List<Exercise> exercises = dal.ReadExerciseListFromAthlete(athlete);
            return exercises;
        }
    }
}
