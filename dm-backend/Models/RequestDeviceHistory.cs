using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dm_backend.Models{
    public class RequestDeviceHistory
    {
        public UserName? RequestedUser { get; set;}
        public string? UserMail { get; set;}    
        public int? DeviceID { get ; set ;}
        public string? Serial_number { get; set; }
        public string? DeviceType { get; set;}
        public string? DeviceBrand { get; set;}
        public string? DeviceModel { get; set;}
        public string? RequestStatus { get; set; }
        public Specification? Specs { get; set; }
        public string? AssignedDate { get; set; }
        public string? RequestDate { get; set;}
        public int? AssignDays { get; set; }
        public  string? ReturnDate {get; set;}
        public UserName? DeviceSubmittedAdmin { get; set; }

    }
}
