using System;
using System.Collections.Generic;

namespace dm_backend.EFModels
{
  public partial class AssignDevice
    {
        public int AssignDeviceId { get; set; }
        public int UserId { get; set; }
        public int DeviceId { get; set; }
        public int AssignedBy { get; set; }
        public int ReturnTo { get; set; }
        public int StatusId { get; set; }
        public DateTime AssignedDate {get; set ;}
        public DateTime ReturnDate {get; set ;}
    

        public Device Device { get; set; }
        public User User { get; set; }
    }
  }
