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

        public Machine()
        {
        }
        public static List<Machine> ReadMachineLocation()
        {
            DAL dal = new DAL();
            List<Machine> machinelocation = dal.ReadMachineLocation();
            return machinelocation;
              
        }

        public static void UpdateMachineLocation(Machine machine, Location location)
        {
            DAL dal = new DAL();
            dal.UpdateMachineLocation(machine, location);
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

        public void UpdateMachine()
        {
            DAL dal = new DAL();
            dal.UpdateMachine(this);
        }

        public bool DeleteMachine(Machine machine)
        {
            DAL dal = new DAL();
            bool isDeleted = dal.DeleteMachine(machine);
            return isDeleted;
        }
    }
}

