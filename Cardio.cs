using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zuydfit
{
    public class Cardio : Exercise
    {
        public string Duration { get; set; }
        public string Distance { get; set; }

        public Cardio(int id, string name, string duration, string distance) : base(id, name)
        {
            Duration = duration;
            Distance = distance;
        }

    }
}
