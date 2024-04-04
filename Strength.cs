using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessApp
{
    public class Strength : Exercise
    {
        public List<Set> Sets { get; set; }

        public Strength(string name) : base(name)
        {
            Sets = new List<Set>();
        }
        
        public Strength(string name, List<Set> sets) : base(name)
        {
            Sets = sets;
        }
    }
}
