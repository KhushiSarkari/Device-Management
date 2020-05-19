using System.Collections.Generic;
using System;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using dm_backend.Models;

namespace dm_backend
{
    public class PartialUserModel : BaseEntity
    {
        public int UserId {get; set;}
        public string? Salutation { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public string? DepartmentName { get; set; }
        public string? DesignationName { get; set; }
        public string? Email { get; set; }
        public string? DOB { get; set; }
        public string? Gender { get; set; }
        public string? DOJ { get; set; }

        internal AppDb Db { get; set; }

        public PartialUserModel()
        {
        }

        internal PartialUserModel(AppDb db)
        {
            Db = db;
        }
       
        
    }
}
