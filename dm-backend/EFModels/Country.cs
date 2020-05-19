using System;
using System.Collections.Generic;

namespace dm_backend.EFModels
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
        public int CountryCode { get; set; }

        public ICollection<ContactNumber> ContactNumber { get; set; }
        public ICollection<State> State { get; set; }
    }
}
