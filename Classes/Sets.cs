using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zuydfit.DataAccessLayer;

namespace Zuydfit
{
    public class Sets
    {
        public int Id { get; set; }
        public int Reps { get; set; }
        public double Weight { get; set; }

        public Sets(int id, int reps, double weight)
        {
            Id = id;
            Reps = reps;
            Weight = weight;
        }

        public Sets ReadSet(int id)
        {
            DAL dal = new DAL();
            Sets returnedSet = dal.ReadSet(id);
            return returnedSet;
        }

        public Sets CreateSet()
        {
            DAL dal = new DAL();
            Sets createdSet = dal.CreateSet(this);
            return createdSet;
        }

        public Sets UpdateSet()
        {
            DAL dal = new DAL();
           Sets returnedSet = dal.UpdateSet(this);
            return returnedSet;
        }

        public void DeleteSet()
        {
            DAL dal = new DAL();
            dal.DeleteSet(Id);
        }
    }
}
    

