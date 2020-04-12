﻿using System;
using System.Collections.Generic;

namespace dm_backend.EFModels
{
    public partial class User
    {
        public User()
        {
            Address = new HashSet<Address>();
            AssignDevice = new HashSet<AssignDevice>();
            Complaints = new HashSet<Complaints>();
            ContactNumber = new HashSet<ContactNumber>();
            Notification = new HashSet<Notification>();
            RequestDevice = new HashSet<RequestDevice>();
            RequestHistory = new HashSet<RequestHistory>();
            UserToAddress = new HashSet<UserToAddress>();
            UserToDependent = new HashSet<UserToDependent>();
            UserToRole = new HashSet<UserToRole>();
        }

        public int UserId { get; set; }
        public int? SalutationId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public int? DepartmentDesignationId { get; set; }
        public string Email { get; set; }
        public int? GenderId { get; set; }
        public int? Status { get; set; }
        public string Hashpassword { get; set; }
        public string Saltpassword { get; set; }

        public DepartmentDesignation DepartmentDesignation { get; set; }
        public Gender Gender { get; set; }
        public Salutation Salutation { get; set; }
        public Status StatusNavigation { get; set; }
        public BankDetails BankDetails { get; set; }
        public ICollection<Address> Address { get; set; }
        public ICollection<AssignDevice> AssignDevice { get; set; }
        public ICollection<Complaints> Complaints { get; set; }
        public ICollection<ContactNumber> ContactNumber { get; set; }
        public ICollection<Notification> Notification { get; set; }
        public ICollection<RequestDevice> RequestDevice { get; set; }
        public ICollection<RequestHistory> RequestHistory { get; set; }
        public ICollection<UserToAddress> UserToAddress { get; set; }
        public ICollection<UserToDependent> UserToDependent { get; set; }
        public ICollection<UserToRole> UserToRole { get; set; }
    }
}
