using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dm_backend.Models
{
    public class FaultyDeviceModel
    {
        public int complaintId { get; set; }
        public int deviceId { get; set; }
        public string deviceType {get; set;}
        public string deviceBrand { get; set; }
        public string deviceModel { get; set; }
        public string serialNumber { get; set; }
        public int userId { get; set; }
        public string salutation { get; set; }
        public name userName { get; set; }
        public string complaintDate { get; set; }
        public string Issue { get; set; }
        public string status { get; set; }
        public string image { get; set;}
       
    }
}
