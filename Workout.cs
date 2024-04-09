using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zuydfit.DataAccessLayer;

namespace Zuydfit
{
    public class Workout
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public List<Exercise> Exercises { get; set; }

        public Workout(int id, DateTime date)
        {
            Id = id;
            Date = date;
            Exercises = new List<Exercise>();
        }

        public Workout(int id, DateTime date, List<Exercise> exercises)
        {
            Id = id;
            Date = date;
            Exercises = exercises;
        }

        public static List<Workout> ReadWorkouts()
        {
            DAL dal = new DAL();
            List<Workout> workouts = dal.ReadWorkouts();
            return workouts;
        }
        
        public Workout ReadWorkout(Workout workout)
        {
            DAL dal = new();
            Workout returnedWorkout = dal.ReadWorkout(workout);
            return returnedWorkout;
        }

        public Workout CreateWorkout(Workout workout)
        {
            DAL dal = new();
            Workout createdWorkout = dal.CreateWorkout(workout);
            return createdWorkout;
        }

        public Workout UpdateWorkout(Workout workout)
        {
            DAL dal = new DAL();
            Workout returnedWorkout = dal.UpdateWorkout(workout);
            return returnedWorkout;
        }

        public static bool DeleteWorkout(Workout workout)
        {
            DAL dal = new DAL();
            bool status = dal.DeleteWorkout(workout);
            return status;
        }


    }
}
