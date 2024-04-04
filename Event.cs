using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Zuydfit;

namespace Zuydfit
{
    public class Event
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Duration { get; set; }
    public List<Athlete> Athletes { get; set; }

    public Event(int id, string name, string duration, List<Athlete> athletes)
    {
        Id = id;
        Name = name;
        Duration = duration;
        Athletes = athletes;
    }
}
}
