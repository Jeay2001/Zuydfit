using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zuydfit
{
    public class Strength : Exercise
    {
        public List<Sets> Sets { get; set; }

        public Strength(int id, string name) : base(id, name)
        {
            Sets = new List<Sets>();
        }
        
        public Strength(int id, string name, List<Sets> sets) : base(id, name)
        {
            Sets = sets;
        }
    }
}
