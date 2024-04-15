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

        public Activity(int id, string name, string duration, List<Athlete> athletes)
        {
            Id = id;
            Name = name;
            Duration = duration;
            Athletes = athletes;
        }
        // Lege constructor voor het maken van nieuwe instanties zonder ID
        public Activity(string name, string duration, List<Athlete> athletes)
        {
            Name = name;
            Duration = duration;
            Athletes = athletes;
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
    }
}
