﻿using System;
using System.Collections.Generic;

namespace dm_backend.EFModels
{
    public partial class ContactType
    {
        public ContactType()
        {
            ContactNumber = new HashSet<ContactNumber>();
        }

        public int ContactTypeId { get; set; }
        public string ContactType1 { get; set; }

        public ICollection<ContactNumber> ContactNumber { get; set; }
    }
}
