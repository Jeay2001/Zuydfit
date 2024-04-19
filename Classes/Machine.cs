using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zuydfit.DataAccessLayer;

namespace Zuydfit
{
    public class Machine
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Location Location { get; set; }

        public Machine(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public void UpdateMachineLocation()
        {
            DAL dal = new DAL();
            dal.UpdateMachineLocation(this);
        }

        public static List<Machine> ReadMachines()
        {
            DAL dal = new DAL();
            List<Machine> machines = dal.ReadMachines();
            return machines;
        }

        public Machine ReadMachine(int machineId)
        {
            DAL dal = new DAL();
            Machine returnedMachine = dal.ReadMachine(machineId);
            return returnedMachine;
        }

        public Machine CreateMachine(Machine machine)
        {
            DAL dal = new DAL();
            Machine createdMachine = dal.CreateMachine(machine);
            return createdMachine;
        }

        public Machine UpdateMachine(Machine machine)
        {
            DAL dal = new DAL();
            Machine updatedMachine = dal.UpdateMachine(machine);
            return updatedMachine;
        }

        public bool DeleteMachine(Machine machine)
        {
            DAL dal = new DAL();
            bool isDeleted = dal.DeleteMachine(machine);
            return isDeleted;
        }
    }
}

