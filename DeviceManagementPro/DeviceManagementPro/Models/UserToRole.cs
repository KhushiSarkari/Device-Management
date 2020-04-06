﻿using System;
using System.Collections.Generic;

namespace DeviceManagementPro.Models
{
    public partial class UserToRole
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }

        public Role Role { get; set; }
        public User User { get; set; }
    }
}
