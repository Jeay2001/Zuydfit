using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zuydfit.DataAccessLayer;

namespace Zuydfit
{
    public abstract class Person
    {
        // Properties
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string StreetName { get; set; }
        public string HouseNumber { get; set; }
        public string PostalCode { get; set; }

        // Constructor
        public Person(int id, string firstName, string lastName, string streetName, string houseNumber, string postalcode)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            StreetName = streetName;
            HouseNumber = houseNumber;
            PostalCode = postalcode;
        }

        public static List<Person> GetPersons() 
        {
            DAL dal = new DAL();
            return dal.GetPerson();
        }
    }
}