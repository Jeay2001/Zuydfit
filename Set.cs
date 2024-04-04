using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zuydfit
{
    public class Set
    {
        public int Id { get; set; }
        public int Reps { get; set; }
        public double Weight { get; set; }


        public Set(int id, int reps, double weight)
        {
            Id = id;
            Reps = reps;
            Weight = weight;
        }  
    }
}
