using System;
using System.Collections.Generic;

namespace dm_backend.EFModels
{
    public partial class Status
    {
        public Status()
        {
            Complaints = new HashSet<Complaints>();
            Device = new HashSet<Device>();
            RequestHistory = new HashSet<RequestHistory>();
            User = new HashSet<User>();
        }

        public int StatusId { get; set; }
        public string StatusName { get; set; }

        public ICollection<Complaints> Complaints { get; set; }
        public ICollection<Device> Device { get; set; }
        public ICollection<RequestHistory> RequestHistory { get; set; }
        public ICollection<User> User { get; set; }

    }
}
