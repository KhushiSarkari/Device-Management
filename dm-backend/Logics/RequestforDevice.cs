
using dm_backend.Models;

using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

using System.Threading.Tasks;

namespace dm_backend.Logics
{
    public class Request
    {
        public string name;
        public string email;
      
      internal AppDb Db { get; set; }
      public  Request(AppDb db)
        {
            Db = db;
        }
        public Request()
        {

        }

         public RequestDeviceHistory addDevice(DeviceRequest  req)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText= "insert_request";
            cmd.CommandType = CommandType.StoredProcedure;
          
            try
                {
                BindRequestProcedureParams(cmd, req);
               return bindReturnData(cmd.ExecuteReader());
                
               
            }   
            catch
           {
                return null;
            }
           
        }

        private void BindRequestProcedureParams(MySqlCommand cmd, DeviceRequest req)
        {
            cmd.Parameters.Add(new MySqlParameter("var_user_id", req.userId));
            cmd.Parameters.Add(new MySqlParameter("var_device_model", req.model));
            cmd.Parameters.Add(new MySqlParameter("var_device_brand", req.brand));
            cmd.Parameters.Add(new MySqlParameter("var_device_type", req.devicetype));
            cmd.Parameters.Add(new MySqlParameter("var_specification_id", req.specificationId));
            cmd.Parameters.Add(new MySqlParameter("var_no_of_days", req.days));
            cmd.Parameters.Add(new MySqlParameter("var_comment", req.comment));
        }

        public RequestDeviceHistory bindReturnData(DbDataReader reader)
        {
            using (reader)
            {
         
                if (reader.Read())
                {
                  return(new RequestDeviceHistory()
                    {

                        RequestedUser = new UserName()
                        {
                            salutation = reader.GetString("salutation"),
                            firstName = reader.GetString("first_name"),
                            middleName = (reader.IsDBNull("middle_name") ? "" : (reader.GetString("middle_name") + " ")),
                            Lastname = reader.GetString("last_name"),
                        },
                        Specs = new BindRequestData().FillDeviceSpecifications(reader),
                    });
                       
                   
                }
                return null;
            }

        }
      


       
        public async Task sendMailToAdmin(  DeviceRequest req , RequestDeviceHistory model)
        {

            List<Request> admins =  new GetAllAdmin(Db).getAllAdmin();


            string body = "";
            string name = model.RequestedUser.salutation + " " + model.RequestedUser.firstName + " " + model.RequestedUser.middleName + " "
                + model.RequestedUser.Lastname;
            string specs = model.Specs.RAM == "" ? "" : "RAM " + model.Specs.RAM + " ";
            specs += model.Specs.Storage == "" ? "" : "Storage " + model.Specs.Storage + " ";
            specs += model.Specs.ScreenSize == "" ? "" : "Screen Size " + model.Specs.ScreenSize + " ";
            specs += model.Specs.Connectivity == "" ? "" : "Connectivity " + model.Specs.Connectivity + " ";
            foreach (Request val in admins)
            {
                
              body = val.name +" <br /> This mail is to inform you that user <b>"+name +"</b> Requestred for device (<b>" + req.devicetype + " " + req.brand + " " + req.model + "</b>) having specification "
                    +" (<b> "+specs +"<b>) <br> Thank You";
                await ((new sendMail().sendNotification(val.email, body, "Device Request Raise")));
            }

         
        }


      
     
    }
}
