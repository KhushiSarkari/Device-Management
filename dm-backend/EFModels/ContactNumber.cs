using System;
using System.Collections.Generic;

namespace dm_backend.EFModels
{
    public partial class ContactNumber
    {
        public ContactNumber()
        {
            Dependent = new HashSet<Dependent>();
        }

        public int ContactId { get; set; }
        public int ContactTypeId { get; set; }
        public int CountryId { get; set; }
        public string AreaCode { get; set; }
        public string Number { get; set; }
        public int UserId { get; set; }

        public ContactType ContactType { get; set; }
        public Country Country { get; set; }
        public User User { get; set; }
        public ICollection<Dependent> Dependent { get; set; }

        internal string GetSafeString(string number)
        {
            throw new NotImplementedException();
        }
    }
}
