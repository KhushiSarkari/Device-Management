using dm_backend.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace dm_backend.Logics
{
    public class SendNotificationMail
    {
        public string name;
        public string email;
        public string deviceType;
        public string deviceName;
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
                    x.deviceType = reader.GetString("type");
                    x.deviceName = reader.GetString("brand") + " " + reader.GetString("model");

                }
                return x;
            }
            return null;
        }

    }
}
