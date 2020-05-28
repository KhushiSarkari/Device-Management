using System;
using System.Collections.Generic;
using System.Linq;
using dm_backend.Models;

using System.Runtime.Serialization;


namespace dm_backend.Data
{
    public class DeviceRepository : IDeviceRepository
    {
    
        private readonly EFDbContext _context;
        public DeviceRepository(EFDbContext context)
        {
            _context = context;
        }

     


        public List<FaultyDeviceModel> getFaultyDevice(string search, string serialNumber, string sortAttribute, string direction)
        {
            var complaintList =from c in _context.Complaints
            join u in _context.User on c.EmployeeId equals u.UserId
            join d in _context.Device on c.DeviceId equals d.DeviceId
            join dt in _context.DeviceType on d.DeviceTypeId equals dt.DeviceTypeId
            join db in _context.DeviceBrand on d.DeviceBrandId equals db.DeviceBrandId
            join dm in _context.DeviceModel on d.DeviceModelId equals dm.DeviceModelId
            join st in _context.Status on c.ComplaintStatusId equals st.StatusId
            join s in _context.Salutation on u.SalutationId equals s.SalutationId into abc
            from bcd in abc.DefaultIfEmpty()
            where st.StatusName == "Unresolved"
            select new FaultyDeviceModel
            {
                userId = u.UserId,
                complaintId = c.ComplaintId,
                salutation = bcd.SalutationName,
                deviceId = d.DeviceId,
                     serialNumber = d.SerialNumber,
                        device = dt.Type+" "+db.Brand+" "+dm.Model,
                        name =(!string.IsNullOrEmpty(bcd.SalutationName) ? bcd.SalutationName+" " :"")+ u.FirstName+ " " +(!string.IsNullOrEmpty(u.MiddleName) ? u.MiddleName+" " :"")+u.LastName,
                        complaintDate =c.ComplaintDate,
                        Comments = c.Comments,
                        image = System.Text.ASCIIEncoding.ASCII.GetString((byte[])c.Image ??new byte[0])                      
            };
             if(!string.IsNullOrEmpty(serialNumber))
             {
             complaintList= complaintList.Where(c => c.serialNumber==serialNumber);            
             }

            else if(!string.IsNullOrEmpty(search))
             {
             complaintList = complaintList.Where(c => (c.name).ToLower().Contains(search.ToLower()) || (c.device).ToLower().Contains(search.ToLower()));            
             }
            
             var complaintLists = complaintList.ToList();      
             if(!string.IsNullOrEmpty(sortAttribute))
             {
                 if(direction == "asc")
                 {
                   complaintLists =complaintLists.OrderBy(c => getValue(c,sortAttribute)).ToList();
                 }
                 else
                 {
                     complaintLists =complaintLists.OrderByDescending(c =>  getValue(c,sortAttribute)).ToList();
                 }
             }     
            return complaintLists.ToList();
        } 

        public string MarkFaultyRequest(int complaintId)
        {
           var fault=(from c in _context.Complaints
           where c.ComplaintId == complaintId
           select c).SingleOrDefault();

           var device=(from d in _context.Device
           where d.DeviceId == fault.DeviceId
           select d).SingleOrDefault();
            
           int complaintstatus = (from s in _context.Status
           where s.StatusName == "Resolved"
           select s.StatusId).First();

           int devicestatus = (from s in _context.Status
           where s.StatusName == "Faulty"
           select s.StatusId).First();
           
           fault.ComplaintStatusId = complaintstatus;
           device.StatusId = devicestatus;
           _context.SaveChanges();
           return "Request Sent";

        }
        public string ResolveRequest(int complaintId)
        {           
           var resolve=(from c in _context.Complaints
           where c.ComplaintId == complaintId
           select c).SingleOrDefault();

           int status = (from s in _context.Status
           where s.StatusName == "Resolved"
           select s.StatusId).First();
           
           resolve.ComplaintStatusId = status;
           resolve.ResolveDate = DateTime.Now;
           _context.SaveChanges();
           return "Request Sent";
        }
        private object getValue(object src,string propertyName)
        {
               Type myType = src.GetType();
                var myPropInfo = myType.GetProperties();             
               var myPropInfoss=myPropInfo.Where(p => Attribute.IsDefined(p, typeof(DataMemberAttribute))).ToList();      
                var myPropInfos=myPropInfo.First(p => ((DataMemberAttribute)Attribute.GetCustomAttribute( p, typeof(DataMemberAttribute))).Name == propertyName);                 
                return myPropInfos.GetValue(src, null);
      }
      
    }
}
