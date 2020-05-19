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
                                         from ad in _context.AssignDevice 
                                         join dev in _context.Device on ad.DeviceId equals dev.DeviceId
                                         join u in _context.User on ad.UserId equals u.UserId
                                        
                                        
                                         select new devices
                                         {
                                             device_id = d.DeviceId,
                                             type = dt.Type,
                                             brand = db.Brand,
                                             model = dm.Model,
                                             color = d.Color,
                                             price = d.Price,
                                             serial_number = d.SerialNumber,
                                             warranty_year = d.WarrantyYear.ToString(),
                                             status = st.StatusName,
                                             assign_date = ad.AssignedDate.ToString(),
                                             return_date = ad.ReturnDate.ToString(),
                                             specifications = new Models.Specification
                                             {
                                                 RAM = s.Ram,
                                                 Connectivity = s.Connectivity,
                                                 ScreenSize = s.ScreenSize,
                                                 Storage = s.Storage
                                             },

                                               assign_by = new name{
                                                   first_name = u.FirstName,
                                                   middle_name = u.MiddleName,
                                                   last_name = u.LastName
                                               },
                                              assign_to = new name{
                                                  first_name = u.FirstName,
                                                  middle_name = u.MiddleName,
                                                  last_name = u.LastName
                                              }

                                         }).ToList();
                                         Console.WriteLine(data[0]);
            return data;
        }

    }
}