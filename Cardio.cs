using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zuydfit
{
    public class Cardio : Exercise
    {
        public int Duration { get; set; }
        public int Distance { get; set; }

        public Cardio(int id, string name, int duration, int distance) : base(id, name)
        {
            Duration = duration;
            Distance = distance;
        }

    }
}
