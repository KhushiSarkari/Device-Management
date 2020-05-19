using System;
using System.Collections.Generic;

namespace dm_backend.EFModels
{
    public partial class Designation
    {
        public Designation()
        {
            DepartmentDesignation = new HashSet<DepartmentDesignation>();
        }

        public int DesignationId { get; set; }
        public string DesignationName { get; set; }

        public ICollection<DepartmentDesignation> DepartmentDesignation { get; set; }
    }
}
