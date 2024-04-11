using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }

}

