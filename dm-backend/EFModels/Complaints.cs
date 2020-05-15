using System;
using System.Collections.Generic;

namespace dm_backend.EFModels
{
    public partial class Complaints
    {
         public int ComplaintId { get; set; }
        public int EmployeeId { get; set; }
        public int DeviceId { get; set; }
        public string Comments { get; set; }
        public byte[] Image { get; set; }
        public int? ComplaintStatusId { get; set; }
        public string ImagePath { get; set; }

        public Status ComplaintStatus { get; set; }
        public Device Device { get; set; }
        public User Employee { get; set; }
    }
}
