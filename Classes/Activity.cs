using System;
using System.Collections.Generic;
using Zuydfit.DataAccessLayer;

namespace Zuydfit
{
    public class Activity 
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Duration { get; set; }
        public List<Athlete> Athletes { get; set; }

        public Activity(int id, string name, string duration)
        {
            Id = id;
            Name = name;
            Duration = duration;
            Athletes = new List<Athlete>();
        }

        public Activity(int id, string name, string duration, List<Athlete> athletes)
        {
            Id = id;
            Name = name;
            Duration = duration;
            Athletes = athletes;
        }

        public Activity(string name, string duration, List<Athlete> athletes)
        {
            Name = name;
            Duration = duration;
            Athletes = athletes;
        }

        public static List<Activity> ReadAthleteActivities(Athlete athlete)
        {
            DAL dal = new DAL();
            List<Activity> returnedActivities = dal.ReadAthleteActivities(athlete);
            return returnedActivities;
        }

        public void AddAthlete(Athlete athlete)
        {
            // Checks if the athlete is already in the list
            foreach (Athlete a in Athletes)
            {
                if (a.Id == athlete.Id)
                {
                    return;
                }
            }
            DAL dal = new DAL();
            dal.AddAthleteToActivity(this, athlete);
            Athletes.Add(athlete);
        }

        public Activity ReadActivity(int id)
        {
            DAL dal = new DAL();
            Activity returnedActivity = dal.ReadActivity(id);
            return returnedActivity;
        }

        public Activity CreateActivity()
        {
            DAL dal = new DAL();
            Activity createdActivity = dal.CreateActivity(this);
            return createdActivity;
        }

        public Activity UpdateActivity()
        {
            DAL dal = new DAL();
            Activity returnedActivity = dal.UpdateActivity(this);
            return returnedActivity;
        }

        public void DeleteActivity()
        {
            DAL dal = new DAL();
            dal.DeleteActivity(this);
        }

        public static List<Activity> ReadAllActivities()
        {
            DAL dal = new DAL();
            List<Activity> returnedActivities = dal.ReadAllActivities();
            return returnedActivities;
        }

        public static List <Activity> ReadActivitieMembers() 
        { 
            DAL dal = new DAL();
            List<Activity> activities = dal.ReadActivitieMembers();
            return activities;
        }

    }
}
