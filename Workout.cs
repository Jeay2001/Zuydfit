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

        public Workout DuplicateWorkout(Athlete athlete)
        {
            DAL dal = new DAL();
            Workout duplicatedWorkout = dal.DuplicateWorkout(this, athlete);
            return duplicatedWorkout;
        }

        public static List<Workout> ReadWorkouts(Athlete athlete)
        {
            DAL dal = new DAL();
            List<Workout> workouts = dal.ReadWorkouts(athlete);
            return workouts;
        }
        
        public Workout ReadWorkout(Workout workout)
        {
            DAL dal = new();
            Workout returnedWorkout = dal.ReadWorkout(workout);
            return returnedWorkout;
        }

        public Workout CreateWorkout(Workout workout, Athlete athlete)
        {
            DAL dal = new();
            Workout createdWorkout = dal.CreateWorkout(workout, athlete);
            return createdWorkout;
        }

        public bool DeleteWorkout()
        {
            DAL dal = new DAL();
            bool status = dal.DeleteWorkout(this);
            return status;
        }


    }
}
