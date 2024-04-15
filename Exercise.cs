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

        public Exercise CreateExercise(Workout workout, Exercise exercise)
        {
            DAL dal = new DAL();
            Exercise createdExercise = dal.CreateExercise(workout, exercise);
            return createdExercise;
        }


        public bool DeleteExercise()
        {
            DAL dal = new DAL();
            bool removed = dal.DeleteExercise(this);
            return removed;
        }

    }
}
