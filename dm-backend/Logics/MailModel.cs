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
    public class MailModel

    {
        public int SalutationId;
        public string? FirstName;
        public string? MiddleName;
        public string? LastName;
        public string? Email;
        public string? DeviceType;
        public string? DeviceBrand;
        public string? DeviceModel;
        // internal AppDb Db;
        // public string querry = @"select st.salutation ,  u.first_name , u.middle_name  , u.last_name ,  u.email , dt.type  ,db.brand , dm.model 
        //                     from device as d inner join device_brand as db using(device_brand_id)
        //                      inner join device_type as dt using(device_type_id) 
        //                      inner join device_model as dm using(device_model_id)
        //                      inner join assign_device using(device_id) 
        //                      inner join user as u using(user_id)
        //                      inner join salutation  as st using (salutation_id) 
        //                      where d.device_id=";
        // public MailModel(AppDb db)
        // {
        //     Db = db;
        // }
        
        // public async Task<string> sendMultipleMail(MultipleNotifications item)
        // {
        //     var body = "";

        //     foreach (NotificationModel device in item.notify)
        //     {

        //     //     var user = await getUserDetails(device.deviceId);
        //     //    body  =  "" + user.first_name +" "+user.middle_name+" "+user.last_name+" " + "<br> <br> This mail is to inform you that  some of our worker need device that you have i.e( <b>  " + user.deviceType + " " +  user.device_brand+" "+user.device_model +
        //     //        "</b>) if you are done  with your work  Kindly return the device to admin so Others can use it <br><br>  Thank You <br> Admin";

        //     //     await (new sendMail().sendNotification(user.email , body,"Device Notification"));

        //     }
        //     return "";
        // }
        // public async Task<MailModel> getUserDetails(int deviceId)
        // {
        //     // var tempQuerry = querry + "" + deviceId + ";";

        //     // using var cmd = Db.Connection.CreateCommand();

        //     // cmd.CommandText = tempQuerry;
        //     // cmd.CommandType = CommandType.Text;
        //     // return await bindResult(await cmd.ExecuteReaderAsync());
        // }
        
        // public async Task sendAcceptNotifYRequest(int userid , int deviceid)
        // {
        //     string body;
        //     var allAdmins = new GetAllAdmin(Db).getAllAdmin();
        //     var user = UserAcceptnotification(userid, deviceid);

        //     foreach (Request val in allAdmins)
        //     {

        //         body = val.name + " <br /> This mail is to inform you that user <b>" + user.first_name +" "+user.middle_name+" "+user.last_name+" " + "</b> Ready to return device  i.e.. (<b>" + user.deviceType + " " + user.device_brand+" "+user.device_model + "</b>) ";

        //         await ((new sendMail().sendNotification(val.email, body, "Device Return Request")));

        //     }
        // }


      
        // public MailModel UserAcceptnotification(int userId , int deviceId)
        // {
        //     using var cmd = Db.Connection.CreateCommand();

        //     cmd.CommandText = "get_assigned_device_user";
        //     cmd.CommandType = CommandType.StoredProcedure;
        //     BindReturnProcedureParams(cmd , userId , deviceId);
        //     return(getDetails(cmd.ExecuteReader()));

        // }
    }
}
