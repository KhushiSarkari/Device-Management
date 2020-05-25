using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using dm_backend;
using System.Data.Common;
using dm_backend.Data;
using dm_backend.EFModels;

namespace dm_backend.Models
{

  public class devices
    {
        private readonly EFDbContext _context;

        public int DeviceId { get; set; }
        public int StatusId { get; set; }
        public int SpecificationId { get; set; }
        public string Type {get;set;}
        public string Brand {get;set;}
        public string Model { get; set; }
        public string Color { get; set; }
        public string Price { get; set; }
        public string SerialNumber { get; set; }
        public string WarrantyYear { get; set; }
        public string PurchaseDate { get; set; }
        public string EntryDate { get; set; }

        public string Status { get; set; }
        public string Comments { get; set; }
        public Specification Specifications { get; set; }
        public string AssignDate { get; set; }
        
        public string ReturnDate { get; set; }
        public name AssignTo { get; set; }
        public name AssignBy { get; set; }

       

        public devices()
        {
            Specifications = new Specification();
            AssignTo = new name();
            AssignBy = new name();
        }

           internal devices(EFDbContext context)
        {
        
            _context =context;
        }
         public static string GetSafeStrings(string colName)
        {

            return colName != null? colName.ToString() : "";
        }
     

    }



       

       


    public class Assign:devices
    {
        public int UserId { get; set; }
        public int AdminId{get;set;}
       
        public Assign()
        {

        }

     
         


        
    }

  
}
