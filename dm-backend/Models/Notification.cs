using System.Collections.Generic;
using System;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using dm_backend.Models;
using static dm_backend.Utilities.Readers;

namespace dm_backend.Models
{
    public class NotificationModel
    {
        public int? notificationId { get; set; }
        public int userId { get; set; }
        public int deviceId {get;set;}
        public string deviceModel { get; set; }
        public string deviceBrand { get; set; }
        public string deviceType { get; set; }
        public Specifications specs { get; set; }    
        public string notificationDate { get; set; }
        public string status { get; set; }
        public string message { get; set; }
        internal AppDb Db { get; set; }

        public NotificationModel()
        {
        }
        public NotificationModel(AppDb db)
        {
            Db = db;
        }


        public void AddOneNotification(MySqlCommand cmd)
        {
            cmd.CommandText = @"insert into notification(`user_id`,`notification_type`,`device_id`,
	`notification_date`,`status_id`,`message`) (select user_id,'Public',device_id,now(),status.status_id,'Submit Possible?'
	from status, assign_device inner join device using (device_id) where assign_device.device_id=@device_id
    and status.status_name='Pending');"; 
                BindParams(cmd);
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
            
        }
		

        public List<NotificationModel> GetNotifications(int userId,string sortField,string sortDirection,string searchField)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = get_all_notifications+searchQuery;
            if(userId!=-1)
                cmd.CommandText +=@" having user_id="+userId;
            cmd.CommandText +=@" order by "+sortField+" "+sortDirection+";"; 
            cmd.Parameters.AddWithValue("@search_field", searchField); 
            using MySqlDataReader reader =  cmd.ExecuteReader();
            return ReadAll(reader);
        }

         private void BindNotificationProcedureParams(MySqlCommand cmd){
            cmd.Parameters.Add(new MySqlParameter("var_device_model", deviceModel));
            cmd.Parameters.Add(new MySqlParameter("var_device_brand", deviceBrand));
            cmd.Parameters.Add(new MySqlParameter("var_device_type", deviceType));
            cmd.Parameters.Add(new MySqlParameter("specification_id", specs.GetSpecificationID(Db)));
            
        }

        public void BindParams(MySqlCommand cmd) { 
        
            cmd.Parameters.Add(new MySqlParameter("@device_id", deviceId));
           
        }

        private List<NotificationModel> ReadAll(MySqlDataReader reader)
        {
            var notifications = new List<NotificationModel>();
            using (reader)
            {
                while (reader.Read())
                {
                    
                    var notification = ReadNotification(reader);
                    notification.specs = ReadSpecifications(reader);
                    notifications.Add(notification);
                }
            }
            return notifications;
        }
        internal string get_all_notifications=@"select notification_id, user_id, notification_type, device_model.model, 
        device_id,device_type.type, device_brand.brand, specification.*,status.status_name, 
        notification_date, message
        from notification
        inner join status using(status_id)
        inner join user using(user_id)
        inner join device using(device_id)
        inner join device_brand using(device_brand_id)
        inner join device_model using(device_model_id)
        inner join device_type using(device_type_id)
        inner join specification using(specification_id)";

        internal string searchQuery=@" where device_type.type like CONCAT('%', @search_field, '%') or device_model.model like CONCAT('%', @search_field, '%') or device_brand.brand like CONCAT('%', @search_field, '%') group by notification_id";       

    }  

    public class MultipleNotifications
    {
        public List<NotificationModel> notify { get; set; }

        internal AppDb Db { get; set; }

        public MultipleNotifications()
        {   notify = new List<NotificationModel>();
        }

        internal MultipleNotifications(AppDb db)
        {
            Db = db;
        }
        public string AddMultipleNotifications()
        {   using var cmd = Db.Connection.CreateCommand();
            try
            {
                
                foreach (NotificationModel notif in notify)
                {
                    notif.AddOneNotification(cmd);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                Db.Connection.Close();
            }

            return "Insert failed";
        }

    } 
}