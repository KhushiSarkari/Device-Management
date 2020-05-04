using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dm_backend.Models{
    public class UserName
    {
        public int? userId { get; set; }
        public string salutation { get; set; }
        public string firstName { get; set; }
        public string middleName { get; set; }
        public string Lastname { get; set; }
    }
}
