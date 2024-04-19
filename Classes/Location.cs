using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zuydfit.DataAccessLayer;

namespace Zuydfit
{
    public class Location
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string StreetName { get; set; }
        public string HouseNumber { get; set; }
        public string PostalCode { get; set; }
        public List<Machine> Machines { get; set; }


        public Location(int id, string name, string streetName, string houseNumber, string postalCode)
        {
            Id = id;
            Name = name;
            StreetName = streetName;
            HouseNumber = houseNumber;
            PostalCode = postalCode;
            Machines = new List<Machine>();
        }

        public Location(int id, string name, string streetName, string houseNumber, string postalCode, List<Machine> machines)
        {
            Id = id;
            Name = name;
            StreetName = streetName;
            HouseNumber = houseNumber;
            PostalCode = postalCode;
            Machines = machines;
        }


        public static List<Location> ReadLocations()
        {
            DAL dal = new DAL();
            List<Location> locations = dal.ReadLocations();
            return locations;
        }

        public Location ReadLocation(Location location)
        {
            DAL dal = new();
            Location returnedLocation = dal.ReadLocation(location);
            return returnedLocation;
        }

        public Location CreateLocation(Location location)
        {
            DAL dal = new();
            Location returnedLocation = dal.CreateLocation(location);
            return returnedLocation; 
        }

        public Location UpdateLocation(Location location)
        {
            DAL dal = new DAL();
            Location returnedLocation = dal.UpdateLocation(location);
            return returnedLocation;
        }

        public bool DeleteLocation(Location location)
        {
            DAL dal = new DAL();
            bool isDeleted = dal.DeleteLocation(location.Id);
            return isDeleted;
        }
        
        public static List<Location> ReadMachineLocations()
        {
            DAL dal = new DAL();
            List<Location> locations = dal.ReadMachineLocations();
            return locations;
        }
    }
}