using System.Collections.Generic;
using Zuydfit.DataAccessLayer;

namespace Zuydfit
{
    public class Machine
    {
        // Eigenschappen van de Machine-klasse
        public int Id { get; set; }
        public string Name { get; set; }
        public Location Location { get; set; }

        // Constructors van de Machine-klasse
        public Machine(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public Machine()
        {
        }

        // Methode om machine-locatiegegevens op te halen
        public static List<Machine> ReadMachineLocation()
        {
            // Maak een instantie van de DAL-klasse
            DAL dal = new DAL();

            // Roep de methode ReadMachineLocation aan vanuit de DAL-klasse om machine-locatiegegevens op te halen
            List<Machine> machinelocations = dal.ReadMachineLocation();

            // Maak een nieuwe lijst om de machines op te slaan
            List<Machine> machines = new List<Machine>();

            // Loop door de lijst met MachineLocation objecten en haal de bijbehorende machines op
            foreach (MachineLocation machineLocation in machinelocations)
            {
                Machine machine = new Machine().ReadMachine(machineLocation.MachineID);
                machines.Add(machine);
            }

            // Return de lijst met machines
            return machines;
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

