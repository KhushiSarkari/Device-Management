using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using dm_backend.EFModels;
using dm_backend.Logics;
using dm_backend.Models;
using Generic;
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
        public int GetNotification(int id)
        {
            var values = _context.Notification.Count(w => (w.UserId == id)&& (w.StatusId==9) );
            return values;
        }
        public string AddMultipleNotifications(MultipleNotifications notifys)
        {   
            try
            {
                
                foreach (NotificationModel notif in notifys.notify)
                {
                    this.AddNotification(notif);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            
            return "Insert failed";
        }

        public int AddNotification(NotificationModel contents){
            var notif = new List<Notification>(from n in _context.Notification
                        from st in _context.Status
                        from t2 in _context.AssignDevice
                        from d in _context.Device
                        where(d.DeviceId == contents.deviceId && st.StatusName=="Pending")
                        
                        select( new Notification{
                UserId=t2.UserId, NotificationType="Public", DeviceId=contents.deviceId, NotificationDate=DateTime.Now, StatusId = st.StatusId, Message="Submit Possible?"
                        })).ToList();
                        
                var notify = _context.Notification.Add( notif[0]);   
               
            try{
                _context.SaveChanges();
                return 1;
            }catch(Exception){
                return 0;
            }
            
        }
        public List<Notification> GetAllNotifications(int userId,string sortField,string sortDirection,string searchField)
        {   
            var result = from n in _context.Notification
                             join st in _context.Status on n.StatusId equals st.StatusId
                             join us in _context.User on n.UserId equals us.UserId
                             join dv in _context.Device on n.DeviceId equals dv.DeviceId
                             join db in _context.DeviceBrand on dv.DeviceBrandId equals db.DeviceBrandId
                             join dm in _context.DeviceModel on dv.DeviceModelId equals dm.DeviceModelId
                             join dt in _context.DeviceType on dv.DeviceTypeId equals dt.DeviceTypeId
                             join sp in _context.Specification on dv.SpecificationId equals sp.SpecificationId
                            
                             select new Notification()
                             {
                                NotificationId= n.NotificationId,
                                UserId= n.UserId,
                                NotificationType= n.NotificationType,
                                Statusname = st ,
                                Message= n.Message,
                                NotificationDate = n.NotificationDate,
                                DeviceName = dt.Type+db.Brand+dm.Model,
                                Device=new Device{
                                     DeviceModel=dm,
                                    DeviceType= dt,
                                    DeviceBrand=db,
                                    DeviceId=dv.DeviceId,
                                     Specification = new EFModels.Specification
                                     {
                                         SpecificationId = dv.Specification.SpecificationId,
                                    Ram = dv.Specification.Ram,
                                    Storage = dv.Specification.Storage,
                                    ScreenSize = dv.Specification.ScreenSize,
                                    Connectivity = dv.Specification.Connectivity
                                
                                }
                                },
                            };
                            if(userId != -1){
                                result = result.Where(us => us.UserId == userId);
                            }
            
            // int pageNumber = Convert.ToInt32((string)HttpContext.Request.Query["page"]);
            // int pageSize = Convert.ToInt32((string)HttpContext.Request.Query["page-size"]);
            // sortDirection = (sortDirection.ToLower()) == "asc" ? "ASC" : "DESC";
            
            
            //  result = (List<Notification>)result.Where(d => (searchField == "") ? true : EF.Functions.Like(string.Concat(d.Type," ",d.Brand," ",d.Model),$"%{searchField}%")).ToList();

            if(!string.IsNullOrEmpty(searchField))
             {
             result = result.Where(d => (d.DeviceName).ToLower().Contains(searchField.ToLower()));            
             }
            

            var resultList = result;
            if(!string.IsNullOrEmpty(sortField)){
                // if(sortDirection == "asc")
                // {
                switch (sortField)
                {
                    case "device_name":
                        resultList = Sorting.ApplyOrder(resultList,"Device.DeviceType",sortDirection).ApplyOrder(resultList,"Device.DeviceBrand",sortDirection).ApplyOrder(resultList,"Device.DeviceModel",sortDirection);
                        break;
                    case "specification":
                        resultList = resultList.OrderBy(sp => sp.Device.Specification.Ram).ThenBy(sp=> sp.Device.Specification.Storage).ThenBy(sp=> sp.Device.Specification.ScreenSize).ThenBy(sp=> sp.Device.Specification.Connectivity);
                        break;
                    default:
                        resultList = resultList.OrderByDescending(d =>d.NotificationDate);
                        break;
                }  
                // }
                // else{
                //     resultList = resultList.OrderByDescending(d => getValue(d,sortField)).ToList();
                // }
            }         

        return resultList.ToList();
        }
         public List<Task> sendMultipleMail(MultipleNotifications item)
        {
            var body = "";
            var ListTask = new List<Task>();
            foreach (NotificationModel device in item.notify)
            {

                var user = getUserDetailsforNotif(device.deviceId).First();
                body  =  "" + user.FirstName +" "+user.MiddleName+" "+user.LastName+" " + "<br> <br> This mail is to inform you that  some of our worker need device that you have i.e( <b>  " + user.DeviceType + " " +  user.DeviceBrand+" "+user.DeviceModel +
                   "</b>) if you are done  with your work  Kindly return the device to admin so Others can use it <br><br>  Thank You <br> Admin";

                ListTask.Add(MailObj.sendNotification(user.Email , body,"Device Notification"));

            }
            return ListTask;
        }
        public IQueryable<MailModel> getUserDetailsforNotif(int DeviceId)
        {
            var userdetail = from d in _context.Device
                             join db in _context.DeviceBrand on d.DeviceBrandId equals db.DeviceBrandId
                             join dt in _context.DeviceType on d.DeviceTypeId equals dt.DeviceTypeId
                             join dm in _context.DeviceModel on d.DeviceModelId equals dm.DeviceModelId
                             join ad in _context.AssignDevice on d.DeviceId equals ad.DeviceId
                             join u in _context.User on ad.UserId equals u.UserId
                             join s in _context.Salutation on u.SalutationId equals s.SalutationId
                             where d.DeviceId == DeviceId
                             select new MailModel{
                             SalutationId = s.SalutationId,
                             FirstName = u.FirstName,
                             MiddleName = u.MiddleName,
                             LastName = u.LastName,
                             Email = u.Email,
                             DeviceType = dt.Type,
                             DeviceBrand = db.Brand,
                             DeviceModel = dm.Model,

                             };
            return userdetail;   
        }       
    }

        
    }
