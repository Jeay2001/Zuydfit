﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zuydfit.DataAccessLayer;

namespace Zuydfit
{
    public class Feedback
    {
        public int Id { get; set; }
        public string FeedbackMessage { get; set; }

        public DateTime Date { get; set; }

        public Feedback(int id, string feedback, DateTime date)
        {
            Id = id;
            FeedbackMessage = feedback;
            Date = date;
        }



      
        public Feedback ReadFeedback(Feedback feedback)
        {
            DAL dal = new();
            Feedback returnedFeedback = dal.ReadFeedback(feedback.Id);
            return returnedFeedback;
        }
        public List<Feedback> ReadAllFeedback()
        {
            DAL dal = new DAL();
            List<Feedback> feedbacks = dal.ReadAllFeedback();
            return feedbacks;
        }

        public Feedback CreateFeedback(Feedback feedback)
        {
            DAL dal = new();
            Feedback returnedFeedback = dal.CreateFeedback(feedback);
            return returnedFeedback;
        }

        public Feedback UpdateFeedback(Feedback feedback)
        {
            DAL dal = new DAL();
            Feedback returnedFeedback = dal.UpdateFeedback(feedback);
            return returnedFeedback;
        }

        public bool DeleteFeedback(Feedback feedback)
        {
            DAL dal = new DAL();
            bool isDeleted = dal.DeleteFeedback(feedback.Id);
            return isDeleted;
        }

    }

}

