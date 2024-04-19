using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zuydfit
{
    public class Coach : Person
    {
        public List<Athlete> Athletes { get; set; } = new List<Athlete>();
        public List<Feedback> Feedbacks { get; set; } = new List<Feedback>();

        public Coach(int id, string firstName, string lastName, string streetName, string houseNumber, string postalcode, List<Feedback> feedbacks)
            : base(id, firstName, lastName, streetName, houseNumber, postalcode)
        {
            Feedbacks = feedbacks;
        }


    }
}