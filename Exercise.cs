using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zuydfit
{
    public abstract class Exercise
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public Exercise(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
