using System;
using System.Collections.Generic;

namespace dm_backend.EFModels

{



 public partial class Address
    {
        public int AddressId { get; set; }
        public int AddressTypeId { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public int CityId { get; set; }
        public string Pin { get; set; }
        public int UserId { get; set; }

        public City City { get; set; }
        public User User { get; set; }
    }

 }

