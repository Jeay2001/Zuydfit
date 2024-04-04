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

        public Strength(int id, string name) : base(id, name)
        {
            Sets = new List<Set>();
        }
        
        public Strength(int id, string name, List<Set> sets) : base(id, name)
        {
            Sets = sets;
        }
    }
}
