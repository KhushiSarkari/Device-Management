using System;
using System.Collections.Generic;

namespace dm_backend.EFModels
{
    public partial class DeviceModel
    {
        public DeviceModel()
        {
            Device = new HashSet<Device>();
            RequestDevice = new HashSet<RequestDevice>();
            RequestHistory = new HashSet<RequestHistory>();
        }

        public int DeviceModelId { get; set; }
        public string Model { get; set; }

        public ICollection<Device> Device { get; set; }
        public ICollection<RequestDevice> RequestDevice { get; set; }
        public ICollection<RequestHistory> RequestHistory { get; set; }
    }
}
