namespace dm_backend.Models
{
    public class Statistics
    {
        public int totalDevices { get; set; }
        public int freeDevices { get; set; }
        public int faults { get; set; }
        public int assignedDevices { get; set; }
        public int deviceRequests { get; set; }
        public int rejectedRequests { get; set; }

    
    }
}