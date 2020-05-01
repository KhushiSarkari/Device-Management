using dm_backend.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Security.Permissions;
using System.Threading.Tasks;

namespace dm_backend.Logics
{
    public class SendNotificationMail
    {
        public string name;
        public string? email;
        public string? deviceType;
        public string? deviceName;
        internal AppDb Db;
        public string querry = @"select st.salutation ,  u.first_name , u.middle_name  , u.last_name ,  u.email , dt.type  ,db.brand , dm.model from 
                            device as d inner join device_brand as db using(device_brand_id)
                            inner join device_type as dt using(device_type_id)  inner join device_model as dm  
                            using(device_model_id)inner join assign_device using(device_id) inner join user as
                            u using(user_id) inner join salutation  as st using (salutation_id) where d.device_id=";
        public SendNotificationMail(AppDb db)
        {
            Db = db;
        }
        public SendNotificationMail()
        {

        }
        public async Task<string> sendMultipleMail(MultipleNotifications item)
        {
            var body = "";

            foreach (NotificationModel device in item.notify)
            {

                var user = await getUserDetails(device.deviceId);
               body  =  "" + user.name + "<br> <br> This mail is to inform you that  some of our worker need device that you have i.e( <b>  " + user.deviceType + " " + user.deviceName +
                   "</b>) if you have done  with your work  kindly return to admin so Other may utilize it <br><br>  Thank You <br> Admin";

                await (new sendMail().sendNotification(user.email , body,"Device Notification"));

            }
            return "";
        }
        public async Task<SendNotificationMail> getUserDetails(int deviceId)
        {
            var tempQuerry = querry + "" + deviceId + ";";

            using var cmd = Db.Connection.CreateCommand();

            cmd.CommandText = tempQuerry;
            cmd.CommandType = CommandType.Text;
            return await bindResult(await cmd.ExecuteReaderAsync());
        }
        public async Task<SendNotificationMail> bindResult(DbDataReader reader)
        {
            var x = new SendNotificationMail();
            using (reader)
            {
                while (await reader.ReadAsync())
                {


                    x.name = (reader.GetString("salutation") + " " + reader.GetString("first_name") + " " + (reader.IsDBNull("middle_name") ? "" : (reader.GetString("middle_name") + " ")) + reader.GetString("last_name"));
                    x.email = reader.GetString("email");
                    x.deviceType = reader?.GetString("type");
                    x.deviceName = reader?.GetString("brand") + " " + reader?.GetString("model");

                }
                return x;
            }
          
        }



        public async Task sendAcceptNotifYRequest(int userid , int deviceid)
        {
            string body;
            var allAdmins = new GetAllAdmin(Db).getAllAdmin();
            var user = UserAcceptnotification(userid, deviceid);

            foreach (Request val in allAdmins)
            {

                body = val.name + " <br /> This mail is to inform you that user <b>" + user.name + "</b> Ready to return device  i.e.. (<b>" + user.deviceType + " " + user.deviceName + "</b>) ";

               // await ((new sendMail().sendNotification(val.email, body, "Device Return Request")));

                await ((new sendMail().sendNotification("ssrawat@ex2india.com", body, "Device Return Request")));
            }
        }


      
        public SendNotificationMail UserAcceptnotification(int userId , int deviceId)
        {
            using var cmd = Db.Connection.CreateCommand();

            cmd.CommandText = "get_assigned_device_user";
            cmd.CommandType = CommandType.StoredProcedure;
            BindReturnProcedureParams(cmd , userId , deviceId);
            return(getDetails(cmd.ExecuteReader()));

        }

        private void BindReturnProcedureParams(MySqlCommand cmd, int userId , int deviceId )
        {

            cmd.Parameters.Add(new MySqlParameter("var_user_id", userId));
            cmd.Parameters.Add(new MySqlParameter("var_device_id", deviceId));

        }

        private SendNotificationMail getDetails(DbDataReader reader )
        {
            using (reader)
            {
                if(reader.Read())
                {
                    return new SendNotificationMail()
                    {
                        name = (reader.GetString("salutation") + " " + reader.GetString("first_name") + " " + (reader.IsDBNull("middle_name") ? "" : (reader.GetString("middle_name") + " ")) + reader.GetString("last_name")),
                        deviceType = reader?.GetString("type"),
                        deviceName = reader?.GetString("brand") + " " + reader?.GetString("model")
                    };
                }
            }



            return null; 
        }



    }
}
