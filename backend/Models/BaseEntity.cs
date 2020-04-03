﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserManagement.Models
{
    public class BaseEntity
    {
        internal List<string> serializableProperties { get; set; }

        public void SetSerializableProperties(string fields)
        {
            if (!string.IsNullOrEmpty(fields))
            {
                var returnFields = fields.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                serializableProperties = returnFields.ToList();
                return;
            }
            var members = this.GetType().GetMembers();

            serializableProperties = new List<string>();
            serializableProperties.AddRange(members.Select(x => x.Name).ToList());
        }
    }
}
