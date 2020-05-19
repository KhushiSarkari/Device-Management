using System;
using System.Collections.Generic;

namespace dm_backend.EFModels
{
    public class GenericDropdownModel
    {
        public int Id { get; set; }
        public string Value { get; set; }
        internal AppDb Db { get; set; }
        public GenericDropdownModel()
        {
            
        }
         internal GenericDropdownModel(AppDb db)
        {
            Db = db;
        }

    }
}
