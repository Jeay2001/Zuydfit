using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zuydfit
{
    public class Machine
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public Machine(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
