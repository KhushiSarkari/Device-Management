﻿using System;
using System.Collections.Generic;

namespace DeviceManagementPro.Models
{
    public partial class Country
    {
        public Country()
        {
            ContactNumber = new HashSet<ContactNumber>();
            State = new HashSet<State>();
        }

        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public string CountryCode { get; set; }

        public ICollection<ContactNumber> ContactNumber { get; set; }
        public ICollection<State> State { get; set; }
    }
}
