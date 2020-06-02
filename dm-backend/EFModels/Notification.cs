using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace dm_backend.EFModels
{
    public partial class Notification
    {
        public Notification()
        {
        }

        [DataMember]
        public int NotificationId { get; set; }
        [DataMember]
        public int UserId { get; set; }
        [DataMember]
        public string NotificationType { get; set; }
        [DataMember]
        public int DeviceId { get; set; }
        [DataMember]
        public int StatusId { get; set; }
        [DataMember]
        public string Message { get; set; }
        [DataMember(Name = "NotificationDate")]
        public DateTime NotificationDate{get; set;}
        [DataMember(Name = "DeviceName")]
        public string DeviceName{get; set;}
        [DataMember]
        public Device Device { get; set; }
        [DataMember]
        public User User { get; set; }
        [DataMember]
        public Status Status{ get; set;}
    }
}