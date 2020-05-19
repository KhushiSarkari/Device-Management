using System;
using System.Collections.Generic;

namespace dm_backend.EFModels
{
    public partial class Salutation
    {
        public Salutation()
        {
            Dependent = new HashSet<Dependent>();
            User = new HashSet<User>();
        }

        public int SalutationId { get; set; }
        public string SalutationName { get; set; }

        public ICollection<Dependent> Dependent { get; set; }
        public ICollection<User> User { get; set; }
    }
}
