using System;
using System.Runtime.Serialization;

namespace dm_backend.Models
{
    [DataContract]
    public class FaultyDeviceModel
    {
        

        [DataMember]
        public int complaintId { get; set; }
        [DataMember]
        public int deviceId { get; set; }
        [DataMember]
        public string deviceType { get; set; }
        [DataMember]
        public string deviceBrand { get; set; }
        [DataMember(Name="device")]
        public string device { get; set; }
        [DataMember]
        public string deviceModel { get; set; }
        [DataMember(Name="serialNumber")]
        public string serialNumber { get; set; }
        [DataMember]

        public string salutation { get; set; }

        public PartialUserModel userName { get; set; }
        [DataMember(Name="name")]
        public string name{get; set;}
        [DataMember(Name="complaintDate")]
        public DateTime complaintDate { get; set; }
        [DataMember]
        public string Comments { get; set; }
        [DataMember]
        public string status { get; set; }
        [DataMember]
        public string image { get; set; }
   

    }
   
}
