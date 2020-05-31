using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using dm_backend.EFModels;
using dm_backend.Logics;
using dm_backend.Utilities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace dm_backend.Data{
    public class NotificationRepository : INotificationRepository{
        private readonly EFDbContext _context;

        public ISendMail MailObj;
        public NotificationRepository(EFDbContext context, ISendMail mail)
        {
            _context = context;
            MailObj = mail;
        }

        public string RejectNotification(int notificationId)
        {
            var rejectnotif=(from c in _context.Notification
                where c.NotificationId == notificationId
                select c).SingleOrDefault();

                int status = (from s in _context.Status
                where s.StatusName == "Rejected"
                select s.StatusId).First();
                rejectnotif.StatusId = status;
                
                _context.SaveChanges();
                return "Request Sent";
        }
        public int GetNotificationCount(int id)
        {
            var values = _context.Notification.Count(w => (w.UserId == id)&& (w.StatusId==9) );
            return values;
        }
        public string AddMultipleNotifications(List<Notification> notifys)
        {   
            try
            {
                
                foreach (Notification notif in notifys)
                {
                    this.AddNotification(notif);
                    this.SendNotificationAsMail(notif);
                }
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                throw e;
            }
            
            return "Inserted!";
        }

        public async void AddNotification(Notification contents)
        {
            contents.NotificationType = "Public";
            contents.NotificationDate = DateTime.Now;
            contents.Message = "Submit Possible?";
            contents.StatusId = _context.Status.Where(st => st.StatusName == "Allocated").Select(st => st.StatusId).First();
            contents.UserId = _context.AssignDevice.Where(assign => assign.DeviceId == contents.DeviceId && assign.StatusId == contents.StatusId).Select(u => u.UserId).First();
               
            try
            {
                await _context.Notification.AddAsync(contents);
            }
            catch(Exception e)
            {
                throw e;
            }
        }
        public void SendNotificationAsMail(Notification notifDetails)
        {
            var UserToNotify = getUserDetailsforNotif(notifDetails.DeviceId).First();
            string body = UserToNotify.User.FullName + "<br><br>This mail is to inform you that some of our employees need a device that you have i.e (<b>" + UserToNotify.Device.DeviceType.Type + " " +  UserToNotify.Device.DeviceBrand.Brand + " " + UserToNotify.Device.DeviceModel.Model +
                   "</b>) if you are done with your work. Kindly return the device to admin so others can use it.<br><br>Thank You<br>Admin";

            MailObj.sendNotification(UserToNotify.User.Email, body, "Device Notification");
        }
        public List<Notification> GetAllNotifications(BaseQueryParams queryParams)
        {
            var result = _context.Notification
                            .Select(x => new Notification{
                                NotificationId = x.NotificationId,
                                UserId= x.UserId,
                                NotificationType= x.NotificationType,
                                Statusname = x.Statusname,
                                Message= x.Message,
                                NotificationDate = x.NotificationDate,
                                User = new User{
                                    Salutation = x.User.Salutation,
                                    FirstName = x.User.FirstName,
                                    MiddleName = x.User.MiddleName,
                                    LastName = x.User.LastName,
                                    Email = x.User.Email,
                                },
                                Device = new Device{
                                    DeviceId = x.DeviceId,
                                    DeviceModel = new DeviceModel{
                                        DeviceModelId = x.Device.DeviceModel.DeviceModelId,
                                        Model = x.Device.DeviceModel.Model
                                    },
                                    DeviceType = new DeviceType{
                                        DeviceTypeId = x.Device.DeviceType.DeviceTypeId,
                                        Type = x.Device.DeviceType.Type
                                    },
                                    DeviceBrand = new DeviceBrand{
                                        DeviceBrandId = x.Device.DeviceBrand.DeviceBrandId,
                                        Brand = x.Device.DeviceBrand.Brand
                                    },
                                    Specification = x.Device.Specification
                                },
                                DeviceName = x.Device.DeviceType.Type + " " + x.Device.DeviceBrand.Brand + " " + x.Device.DeviceModel.Model
                            });

            if(queryParams.Id.HasValue)
            {
                result = result.Where(us => us.UserId == queryParams.Id);
            }

            if(!string.IsNullOrEmpty(queryParams.Search))
            {
                result = result.Where(d => d.DeviceName.ToLower().Contains(queryParams.Search.ToLower()));            
            } 

            if(!string.IsNullOrEmpty(queryParams.SortField))
            {
                switch (queryParams.SortField)
                {
                    case "device_name":
                        result = result
                                    .OrderBy("Device.DeviceType.Type",queryParams.SortDirection)
                                    .ThenBy("Device.DeviceBrand.Brand",queryParams.SortDirection)
                                    .ThenBy("Device.DeviceModel.Model",queryParams.SortDirection);
                        break;
                    case "specification":
                        result = result
                                    .OrderBy("Device.Specification.Ram", queryParams.SortDirection)
                                    .ThenBy("Device.Specification.Storage", queryParams.SortDirection)
                                    .ThenBy("Device.Specification.ScreenSize", queryParams.SortDirection)
                                    .ThenBy("Device.Specification.Connectivity", queryParams.SortDirection);
                        break;
                    default:
                        result = result.OrderBy(d =>d.NotificationDate);
                        break;
                }
            }         
            return result.ToList();
        }
        public IQueryable<AssignDevice> getUserDetailsforNotif(int DeviceId)
        {
            var UserDetails = _context.AssignDevice
                                .Include(ad => ad.User)
                                .Include(x => x.Device)
                                    .ThenInclude(x => x.DeviceModel)
                                .Include(x => x.Device)
                                    .ThenInclude(x => x.DeviceType)
                                .Include(x => x.Device)
                                    .ThenInclude(x => x.DeviceBrand)
                                .Where(ad => ad.DeviceId == DeviceId);
            return UserDetails;
        }
    }

        
    }
