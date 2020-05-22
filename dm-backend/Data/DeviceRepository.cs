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


    public class DeviceRepository : IDeviceRepository
    {

        private readonly EFDbContext _context;
        public DeviceRepository(EFDbContext context)
        {
            _context = context;
        }

        public List<devices> GetAllDevices(string device_name, string serial_number, string status_name)
        {
            var data = new List<devices>(from d in _context.Device
                                         join dt in _context.DeviceType on d.DeviceTypeId equals dt.DeviceTypeId
                                         join dm in _context.DeviceModel on d.DeviceModelId equals dm.DeviceModelId
                                         join db in _context.DeviceBrand on d.DeviceBrandId equals db.DeviceBrandId
                                         join st in _context.Status on d.StatusId equals st.StatusId
                                         join s in _context.Specification on d.SpecificationId equals s.SpecificationId
                                         join ad in _context.AssignDevice on d.DeviceId equals ad.DeviceId into grouping
                                         from ad in grouping.DefaultIfEmpty()
                                         join u in _context.User on ad.UserId equals u.UserId into groupings
                                         from u in groupings.DefaultIfEmpty()
                                         select new devices
                                         {
                                             DeviceId = d.DeviceId,
                                             Type = devices.GetSafeStrings(dt.Type),
                                             Brand = devices.GetSafeStrings(db.Brand),
                                             Model = devices.GetSafeStrings(dm.Model),
                                             Color = devices.GetSafeStrings(d.Color),
                                             Price = devices.GetSafeStrings(d.Price),
                                             SerialNumber = devices.GetSafeStrings(d.SerialNumber),
                                             EntryDate = devices.GetSafeStrings(d.EntryDate.ToString()),
                                             WarrantyYear = devices.GetSafeStrings(d.WarrantyYear.ToString()),
                                             PurchaseDate = d.PurchaseDate.ToString(),
                                             Status = devices.GetSafeStrings(st.StatusName),
                                             AssignDate = devices.GetSafeStrings(ad.AssignedDate.ToString()),
                                             ReturnDate = devices.GetSafeStrings(ad.ReturnDate.ToString()),
                                             Specifications = new Specification
                                             {
                                                 SpecificationId = s.SpecificationId,
                                                 Ram = devices.GetSafeStrings(s.Ram),
                                                 Connectivity = devices.GetSafeStrings(s.Connectivity),
                                                 ScreenSize = devices.GetSafeStrings(s.ScreenSize),
                                                 Storage = devices.GetSafeStrings(s.Storage)
                                             },

                                             AssignBy = new name
                                             {
                                                 first_name = devices.GetSafeStrings(u.FirstName),
                                                 middle_name = devices.GetSafeStrings(u.MiddleName),
                                                 last_name = devices.GetSafeStrings(u.LastName)
                                             },
                                             AssignTo = new name
                                             {
                                                 first_name = devices.GetSafeStrings(u.FirstName),
                                                 middle_name = devices.GetSafeStrings(u.MiddleName),
                                                 last_name = devices.GetSafeStrings(u.LastName)
                                             }

                                         });
            
            data = (List<devices>)data.Where(d => (serial_number == "") ? true : d.SerialNumber == serial_number)
            .Where(d => (status_name == "") ? true : EF.Functions.Like(d.Status, status_name))
            .Where(d => (device_name == "") ? true : EF.Functions.Like(string.Concat(d.Type," ",d.Brand," ",d.Model),$"%{device_name}%"))
            .ToList();
            

            return data;
        }

        public List<DeviceInsertUpdate> GetDeviceById(int device_id)
        {
            Console.WriteLine(device_id);
            var data = new List<DeviceInsertUpdate>(
                from d in _context.Device
                join dt in _context.DeviceType on d.DeviceTypeId equals dt.DeviceTypeId
                join dm in _context.DeviceModel on d.DeviceModelId equals dm.DeviceModelId
                join db in _context.DeviceBrand on d.DeviceBrandId equals db.DeviceBrandId
                where d.DeviceId == device_id
                select new DeviceInsertUpdate
                {
                    DeviceId = d.DeviceId,
                    Type = devices.GetSafeStrings(dt.Type),
                    Brand = devices.GetSafeStrings(db.Brand),
                    Model = devices.GetSafeStrings(dm.Model),
                    Color = devices.GetSafeStrings(d.Color),
                    Price = devices.GetSafeStrings(d.Price),
                    SerialNumber = devices.GetSafeStrings(d.SerialNumber),
                    EntryDate = devices.GetSafeStrings(d.EntryDate.ToString()),
                    WarrantyYear = devices.GetSafeStrings(d.WarrantyYear.ToString()),
                    PurchaseDate = devices.GetSafeStrings(d.PurchaseDate.ToString()),
                    StatusId = d.StatusId,
                    SpecificationId = d.SpecificationId


                }).ToList();




            return data;
        }

        public List<devices> getDeviceDescriptionbyid(int device_id)
        {
            var data = new List<devices>(from d in _context.Device
                                         join dt in _context.DeviceType on d.DeviceTypeId equals dt.DeviceTypeId
                                         join dm in _context.DeviceModel on d.DeviceModelId equals dm.DeviceModelId
                                         join db in _context.DeviceBrand on d.DeviceBrandId equals db.DeviceBrandId
                                         join st in _context.Status on d.StatusId equals st.StatusId
                                         join s in _context.Specification on d.SpecificationId equals s.SpecificationId
                                         join c in _context.Complaints on d.DeviceId equals c.DeviceId into grouping
                                         from c in grouping.DefaultIfEmpty()
                                         where d.DeviceId == device_id
                                         select new devices
                                         {
                                             DeviceId = d.DeviceId,
                                             Type = devices.GetSafeStrings(dt.Type),
                                             Brand = devices.GetSafeStrings(db.Brand),
                                             Model = devices.GetSafeStrings(dm.Model),
                                             Color = devices.GetSafeStrings(d.Color),
                                             Price = devices.GetSafeStrings(d.Price),
                                             SerialNumber = devices.GetSafeStrings(d.SerialNumber),
                                             EntryDate = devices.GetSafeStrings(d.EntryDate.ToString()),
                                             WarrantyYear = devices.GetSafeStrings(d.WarrantyYear.ToString()),
                                             PurchaseDate = d.PurchaseDate.ToString(),
                                             Status = devices.GetSafeStrings(st.StatusName),
                                             Specifications = new Specification
                                             {
                                                 SpecificationId = s.SpecificationId,
                                                 Ram = devices.GetSafeStrings(s.Ram),
                                                 Connectivity = devices.GetSafeStrings(s.Connectivity),
                                                 ScreenSize = devices.GetSafeStrings(s.ScreenSize),
                                                 Storage = devices.GetSafeStrings(s.Storage)
                                             },
                                             Comments = c.Comments

                                         }).ToList();

            return data;
        }
         public string addDevice(DeviceInsertUpdate d)
            {
                 var device_type_id = (from t in _context.DeviceType
             where t.Type == d.Type
             select t.DeviceTypeId)
             .SingleOrDefault();
             var device_brand_id = (from b in _context.DeviceBrand
             where b.Brand == d.Brand
             select b.DeviceBrandId)
             .SingleOrDefault();
             var device_model_id = (from m in _context.DeviceModel
             where m.Model == d.Model
             select m.DeviceModelId)
             .SingleOrDefault();
           Console.WriteLine(d.SerialNumber);
                 var data = _context.Device.Add(new Device
                   {

                       DeviceTypeId = device_type_id,
                       DeviceBrandId = device_brand_id,
                       DeviceModelId = device_model_id,
                       Color = d.Color,
                       Price = d.Price,
                       SerialNumber = d.SerialNumber,
                       WarrantyYear = Convert.ToSByte(d.WarrantyYear),
                       PurchaseDate = Convert.ToDateTime(d.PurchaseDate),
                        StatusId = d.StatusId,
                       SpecificationId = d.SpecificationId,
                       EntryDate = Convert.ToDateTime(d.EntryDate) 
                   });
                   Console.WriteLine(data);
                try
                {
                    _context.SaveChanges();
                    Console.WriteLine(data);

                    return "Insertion successfull";
                }
                catch (DbUpdateConcurrencyException e)
                {
                    return "Not inserted" + e;
                }
            }
        public string updateDevice(int device_id, DeviceInsertUpdate d)
        {
            var entity = _context.Device.FirstOrDefault(device => device.DeviceId == device_id);
            var device_type_id = (from t in _context.DeviceType
                                  where t.Type == d.Type
                                  select t.DeviceTypeId)
                                  .SingleOrDefault();
            var device_brand_id = (from b in _context.DeviceBrand
                                   where b.Brand == d.Brand
                                   select b.DeviceBrandId)
                                  .SingleOrDefault();
            var device_model_id = (from m in _context.DeviceModel
                                   where m.Model == d.Model
                                   select m.DeviceModelId)
                                  .SingleOrDefault();

            if (entity != null)
            {
                entity.DeviceTypeId = device_type_id;
                entity.DeviceTypeId = device_type_id;
                entity.DeviceBrandId = device_brand_id;
                entity.DeviceModelId = device_model_id;
                entity.Color = d.Color;
                entity.Price = d.Price;
                entity.SerialNumber = d.SerialNumber;
                entity.WarrantyYear = Convert.ToSByte(d.WarrantyYear);
                entity.PurchaseDate = Convert.ToDateTime(d.PurchaseDate);
                entity.StatusId = d.StatusId;
                entity.SpecificationId = d.SpecificationId;
                entity.EntryDate = Convert.ToDateTime(d.EntryDate);

            }
            try
            {
                _context.SaveChanges();
                return "Updation successfull";
            }
            catch (DbUpdateConcurrencyException e)
            {
                return "Not updated" + e;
            }
        }
        public int deleteDevice(int device_id)
        {
            var transaction = _context.Database.BeginTransaction();
            try
            {
                var device = _context.Device.Find(device_id);
                _context.Device.Remove(device);
                _context.SaveChanges();
                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                Console.WriteLine(ex);
            }
            return 1;

        }
        public string assignDevice(Assign a)
        {
             var status_id = (from s in _context.Status
             where s.StatusName == "Allocated"
             select s.StatusId)
             .SingleOrDefault();
               
            
             Console.WriteLine("status_id" +status_id);
             var data =
              _context.AssignDevice.Add(new AssignDevice
              {
                  DeviceId = a.DeviceId,
                  ReturnDate = DateTime.Parse(a.ReturnDate),
                  AssignedDate = DateTime.Now,
                  UserId = a.UserId,
                  AssignedBy = a.AdminId,
                  ReturnTo = a.AdminId,
                  StatusId = status_id
              });
             var entity = _context.Device.FirstOrDefault(device => device.DeviceId == a.DeviceId);
             if (entity != null)
            {
               entity.StatusId = status_id;
                }    

            try
            {
                _context.SaveChanges();
                Console.WriteLine(data);

                return "Device assigned";
            }
            catch (Exception e)
            {
                return "Device assign failed" + e ;
            }
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
                    Storage = s.Storage
                }).ToList();
            return data;
        }

        public List<Specifications> getSpecificationById(int specification_id)
        {
            var data = new List<Specifications>(
                from s in _context.Specification
                where s.SpecificationId == specification_id
                select new Specifications
                {
                    specification_id = s.SpecificationId,
                    RAM = s.Ram,
                    Storage = s.Storage,
                    ScreenSize = s.ScreenSize,
                    Connectivity = s.Connectivity
                }
            ).ToList();
            return data;
        }


        public string addSpecification(Specification s)
        {
            var data =
               _context.Specification.Add(s);
            
            try
            {
                _context.SaveChanges();
                Console.WriteLine(data);

                return "Insertion successfull";
            }
            catch (DbUpdateConcurrencyException e)
            {
                return "Not inserted" + e;
            }
        }

        public string updateSpecification(int specification_id, Specification s)
        {
            var entity = _context.Specification.FirstOrDefault(spec => spec.SpecificationId == specification_id);
            if (entity != null)
            {
                entity.Ram = s.Ram;
                entity.Connectivity = s.Connectivity;
                entity.ScreenSize = s.ScreenSize;
                entity.Storage = s.Storage;
            }
            try
            {
                _context.SaveChanges();
                Console.WriteLine(entity);

                return "Insertion successfull";
            }
            catch (Exception)
            {
                return "Not inserted";
            }
        }

        public async Task<Specification> deleteSpecification(int specification_id)
        {
            var transaction = _context.Database.BeginTransaction();
            try
            {
                var specification = await _context.Specification.FindAsync(specification_id);
                // var device = await _context.Device.FindAsync(specification_id);
                // var RequestDevice = await _context.RequestDevice.FindAsync(specification_id);
                // var RequestHistory = await _context.RequestHistory.FindAsync(specification_id);
                _context.Specification.Remove(specification);
                // _context.Device.Remove(device);
                // _context.RequestDevice.Remove(RequestDevice);
                // _context.RequestHistory.Remove(RequestHistory);
                _context.SaveChanges();
                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                Console.WriteLine(ex);
            }
            return null;
        }

        public string addType(DeviceType t)
        {
            var data =
               _context.DeviceType.Add(new DeviceType
               {
                   Type = t.Type

               });
            try
            {
                _context.SaveChanges();
                Console.WriteLine(data);

                return "Insertion successfull";
            }
            catch (Exception e)
            {
                return "Insertion failed" + e;
            }
        }

        public string addBrand(DeviceBrand b)
        {
            var data =
              _context.DeviceBrand.Add(new DeviceBrand
              {
                  Brand = b.Brand

              });
            try
            {
                _context.SaveChanges();
                Console.WriteLine(data);

                return "Insertion successfull";
            }
            catch (Exception e)
            {
                return "Insertion failed" + e;
            }
        }

        public string addModel(DeviceModel m)
        {
            var data =
               _context.DeviceModel.Add(new DeviceModel
               {
                   Model = m.Model

               });
            try
            {
                _context.SaveChanges();
                Console.WriteLine(data);

                return "Insertion successfull";
            }
            catch (Exception e)
            {
                return "Insertion failed" + e;
            }
        }

        public List<devices> getPreviousDevice(int id, string ToSearch, string ToSort, string Todirection)
        {
             var data = new List<devices>(from d in _context.Device
                                         join dt in _context.DeviceType on d.DeviceTypeId equals dt.DeviceTypeId
                                         join dm in _context.DeviceModel on d.DeviceModelId equals dm.DeviceModelId
                                         join db in _context.DeviceBrand on d.DeviceBrandId equals db.DeviceBrandId
                                         join r in _context.RequestHistory on d.DeviceId equals r.DeviceId
                                         join u in _context.User on r.UserId equals u.UserId
                                        where r.UserId == id
                                         select new devices
                                         {
                
                                            DeviceId = (int)r.DeviceId,
                                             Type = devices.GetSafeStrings(dt.Type),
                                             Brand = devices.GetSafeStrings(db.Brand),
                                             Model = devices.GetSafeStrings(dm.Model),
                                             AssignDate = devices.GetSafeStrings(r.AssignedDate.ToString()),
                                             ReturnDate = devices.GetSafeStrings(r.ReturnDate.ToString()),
                                           
                                         });
              data = (List<devices>)data
             .Where(d => (ToSearch == "") ? true : EF.Functions.Like(string.Concat(d.Type," ",d.Brand," ",d.Model),$"%{ToSearch}%"))
             .ToList();
        
                                         return data;
                   }

        public List<devices> getCurrentDevice(int id, string ToSearch, string ToSort, string Todirection)
        {
             var data = new List<devices>(from d in _context.Device
                                         join dt in _context.DeviceType on d.DeviceTypeId equals dt.DeviceTypeId
                                         join dm in _context.DeviceModel on d.DeviceModelId equals dm.DeviceModelId
                                         join db in _context.DeviceBrand on d.DeviceBrandId equals db.DeviceBrandId
                                         join st in _context.Status on d.StatusId equals st.StatusId
                                         join ad in _context.AssignDevice on d.DeviceId equals ad.DeviceId
                                         join u in _context.User on ad.UserId equals u.UserId 
                                         where ad.UserId == id && st.StatusName == "Allocated"
                                         select new devices
                                         {
                                             DeviceId = ad.DeviceId,
                                             Type = devices.GetSafeStrings(dt.Type),
                                             Brand = devices.GetSafeStrings(db.Brand),
                                             Model = devices.GetSafeStrings(dm.Model),
                                             Status = devices.GetSafeStrings(st.StatusName),
                                             AssignDate = devices.GetSafeStrings(ad.AssignedDate.ToString()),
                                             ReturnDate = devices.GetSafeStrings(ad.ReturnDate.ToString()),
                                           
                                         });
            
            data = (List<devices>)data
             .Where(d => (ToSearch == "") ? true : EF.Functions.Like(string.Concat(d.Type," ",d.Brand," ",d.Model),$"%{ToSearch}%"))
             .ToList();
            

            return data;
           
        }
    }
}