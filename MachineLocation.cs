namespace Zuydfit
{
    public class MachineLocation
    {
        public int MachineID { get; set; }
        public int LocationID { get; set; }

        // Optioneel: Constructor om direct waarden toe te wijzen
        public MachineLocation(int machineId, int locationId)
        {
            MachineID = machineId;
            LocationID = locationId;
        }

        // Optioneel: Default constructor
        public MachineLocation()
        {
        }
    }
}