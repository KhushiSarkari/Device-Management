using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dm_backend.EFModels;
using dm_backend.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace dm_backend.Data
{
    public class EFRepository : IEFRepository
    {
    
        private readonly EFDbContext _context;
        public EFRepository(EFDbContext context)
        {
            _context = context;
        }

       public async Task<Statistics> GetStatus()
        {
            Statistics statisticsObject = new Statistics{
                 totalDevices=await  _context.Device.CountAsync(),
                 assignedDevices=await _context.AssignDevice.CountAsync(),
                 deviceRequests=await _context.RequestDevice.CountAsync(),
                 freeDevices= await _context.Device.Where(ss=> ss.Status.StatusName=="Free").CountAsync(),
                 faults=await _context.Complaints.Where(ss=>ss.ComplaintStatus.StatusName =="Unresolved").CountAsync(),
                 rejectedRequests=await _context.RequestHistory.Where(ss=> ss.Status.StatusName=="Rejected").CountAsync()
               };
               
            return statisticsObject;
        }
    
        public List<devices> GetAllDevices()
        {
            var data = new List<devices>(from d in _context.Device 
                                         join dt in _context.DeviceType on  d.DeviceTypeId equals dt.DeviceTypeId
                                         join dm in _context.DeviceModel on d.DeviceModelId equals dm.DeviceModelId
                                         join db in _context.DeviceBrand on d.DeviceBrandId equals db.DeviceBrandId
                                         join st in _context.Status on d.StatusId equals st.StatusId
                                         join s in _context.Specification on d.SpecificationId equals s.SpecificationId
                                         join ad in _context.AssignDevice on  d.DeviceId equals ad.DeviceId into grouping
                                         from ad in grouping.DefaultIfEmpty()
                                         join u in _context.User on ad.UserId equals u.UserId into groupings
                                         from u in groupings.DefaultIfEmpty()
                                         select new devices
                                         {
                                            device_id = d.DeviceId,
                                             type = devices.GetSafeStrings(dt.Type),
                                             brand = devices.GetSafeStrings(db.Brand),
                                             model = devices.GetSafeStrings(dm.Model),
                                             color = devices.GetSafeStrings(d.Color),
                                             price = devices.GetSafeStrings(d.Price),
                                             serial_number = devices.GetSafeStrings(d.SerialNumber),
                                             entry_date = devices.GetSafeStrings(d.EntryDate.ToString()),
                                             warranty_year = devices.GetSafeStrings(d.WarrantyYear.ToString()),
                                            // purchase_date = d.PurchaseDate.ToString(),
                                             status = devices.GetSafeStrings(st.StatusName),
                                             assign_date = devices.GetSafeStrings(ad.AssignedDate.ToString()),
                                             return_date = devices.GetSafeStrings(ad.ReturnDate.ToString()),
                                             specifications = new Specifications
                                             {
                                                 specification_id = s.SpecificationId,
                                                 RAM = devices.GetSafeStrings(s.Ram),
                                                 Connectivity = devices.GetSafeStrings(s.Connectivity),
                                                 ScreenSize = devices.GetSafeStrings(s.ScreenSize),
                                                 Storage = devices.GetSafeStrings(s.Storage)
                                             },

                                               assign_by = new name{
                                                   first_name = devices.GetSafeStrings(u.FirstName),
                                                   middle_name = devices.GetSafeStrings(u.MiddleName),
                                                   last_name = devices.GetSafeStrings(u.LastName)
                                               },
                                              assign_to = new name{
                                                  first_name = devices.GetSafeStrings(u.FirstName),
                                                  middle_name = devices.GetSafeStrings(u.MiddleName),
                                                  last_name = devices.GetSafeStrings(u.LastName)
                                              }

                                         }).ToList();
                                       
            return data;
        }

        public List<DeviceInsertUpdate> GetDeviceById(int device_id)
        {
            Console.WriteLine(device_id);
            var data = new List<DeviceInsertUpdate> (
                from d in _context.Device
                join dt in _context.DeviceType on  d.DeviceTypeId equals dt.DeviceTypeId
                join dm in _context.DeviceModel on d.DeviceModelId equals dm.DeviceModelId
                join db in _context.DeviceBrand on d.DeviceBrandId equals db.DeviceBrandId
                where d.DeviceId == device_id
                select new DeviceInsertUpdate
                {
                                             device_id = d.DeviceId,
                                             type = devices.GetSafeStrings(dt.Type),
                                             brand = devices.GetSafeStrings(db.Brand),
                                             model = devices.GetSafeStrings(dm.Model),
                                             color = devices.GetSafeStrings(d.Color),
                                             price = devices.GetSafeStrings(d.Price),
                                             serial_number = devices.GetSafeStrings(d.SerialNumber),
                                             entry_date = devices.GetSafeStrings(d.EntryDate.ToString()),
                                             warranty_year = devices.GetSafeStrings(d.WarrantyYear.ToString()),
                                            // purchase_date = devices.GetSafeStrings(d.PurchaseDate.ToString()),
                                             status_id = d.StatusId,
                                             specification_id = d.SpecificationId
                                            
                
                                        }).ToList();
        

                
            
            return data;
        }

        public List<devices> getDeviceDescriptionbyid(int device_id)
        {
             var data = new List<devices>(from d in _context.Device 
                                         join dt in _context.DeviceType on  d.DeviceTypeId equals dt.DeviceTypeId
                                         join dm in _context.DeviceModel on d.DeviceModelId equals dm.DeviceModelId
                                         join db in _context.DeviceBrand on d.DeviceBrandId equals db.DeviceBrandId
                                         join st in _context.Status on d.StatusId equals st.StatusId
                                         join s in _context.Specification on d.SpecificationId equals s.SpecificationId
                                         join c in _context.Complaints on  d.DeviceId equals c.DeviceId into grouping
                                         from c in grouping.DefaultIfEmpty()
                                         where d.DeviceId == device_id
                                         select new devices
                                         {
                                            device_id = d.DeviceId,
                                             type = devices.GetSafeStrings(dt.Type),
                                             brand = devices.GetSafeStrings(db.Brand),
                                             model = devices.GetSafeStrings(dm.Model),
                                             color = devices.GetSafeStrings(d.Color),
                                             price = devices.GetSafeStrings(d.Price),
                                             serial_number = devices.GetSafeStrings(d.SerialNumber),
                                             entry_date = devices.GetSafeStrings(d.EntryDate.ToString()),
                                             warranty_year = devices.GetSafeStrings(d.WarrantyYear.ToString()),
                                            // purchase_date = d.PurchaseDate.ToString(),
                                             status = devices.GetSafeStrings(st.StatusName),
                                             specifications = new Specifications
                                             {
                                                specification_id = s.SpecificationId,
                                                 RAM = devices.GetSafeStrings(s.Ram),
                                                 Connectivity = devices.GetSafeStrings(s.Connectivity),
                                                 ScreenSize = devices.GetSafeStrings(s.ScreenSize),
                                                 Storage = devices.GetSafeStrings(s.Storage)
                                             },
                                            comments = c.Comments
                                                                                  
                                         }).ToList();
                                       
            return data;
        }

        public List<Specifications> getAllSpecifications()
        {
            var data = new List<Specifications>(
            
                from s in _context.Specification
                select new Specifications
                {
                    specification_id = s.SpecificationId,
                    Connectivity = s.Connectivity,
                    RAM = s.Ram,
                    ScreenSize = s.ScreenSize,
                    Storage =s.Storage
                }).ToList();
            return data;
        }

        public List<Specifications> getSpecificationById(int specification_id)
        {
            var data = new List<Specifications>(
                from s in _context.Specification
                where s.SpecificationId == specification_id
                select new Specifications{
                    
                }
            ).ToList();
            return data;
        }
    }
}