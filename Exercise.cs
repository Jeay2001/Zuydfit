using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessApp
{
    public abstract class Exercise
    {
        public string Name { get; set; }


        public Exercise(string name)
        {
            Name = name;
        }
    }
}
