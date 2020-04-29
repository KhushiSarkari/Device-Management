using dm_backend.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace dm_backend.Logics
{
    public class FaultyDevice
    {
        public string querryAll = @"select  c.employee_id ,s.salutation ,   u.first_name , u.middle_name  , u.last_name , 
d.device_id  ,  dt.type , db.brand , dm.model , d.serial_number , c.complaint_id ,
 c.comments , st.status_name , c.complaint_date from complaints as c
inner join status as st on st.status_id = c.complaint_status_id 
inner join user as u on u.user_id = c.employee_id
 inner  join salutation as s using (salutation_id)
inner join device as d using (device_id)
inner join device_model  as dm using(device_model_id)
inner join device_brand as db using(device_brand_id)
inner join device_type as dt using(device_type_id)
where concat(u.first_name , ' ', if (u.middle_name is null, '' ,  concat(u.middle_name, ' ')) , u.last_name ) like concat('%' ,@find ,'%') 
and  if(@status is null ,  st.status_name like '%' OR st.status_name is null  , st.status_name = @status)
and if(@serialNumber is null ,d.serial_number like '%'  OR d.serial_number is null , d.serial_number = @serialNumber ) and st.status_name = 'Unresolved'";
      //  public string getUserID = "  and  u.user_id = @userid)";

        public AppDb Db { get;  }
            
        public FaultyDevice(AppDb db)
        {
            Db = db;
        }



        public static string GetSafeString(MySqlDataReader reader, string colName)
        {
            return reader[colName] != DBNull.Value ? (string)reader[colName] : "";
        }
        public string FindSortingAttribute(string value)
        {

            value = value.ToLower();
            var attribute = value switch
            {
                "user" =>
                        "order by concat(u.first_name , ' ', if (u.middle_name is null, '' , concat(u.middle_name , ' ')) , u.last_name )",

                "serialnumber" =>
                         "order by d.serial_number",

                "device" =>
                         "order by concat(db.brand, ' ', dm.model)",

                 "date" =>
                      "order by c.complaint_date",
                _ =>
                    "order by c.complaint_date"
            };

           return attribute;

        }
         public List<FaultyDeviceModel> getFaultyDevice ( int userId , string search  , string serialNumber  , string status ,string sortAttribute , string direction)
        {

            using var cmd = Db.Connection.CreateCommand();
            var querry = querryAll;

           // if (userId != -1)
         //   {
         //       querry += getUserID;
          //  }

            var attribute = " " + FindSortingAttribute(sortAttribute);

            attribute += (direction == "" ? " asc" : " " + direction);


            querry += attribute;
            // if (page >= 0 && page_size >= 0)
            // {
            //     querry += " limit @offset , @limit ;";
            //     new SortRequestHistoryData(Db).BindLimitParams(cmd, page, page_size);
            // }
            // else
            //     querry += " ;";

            bindParams(cmd, userId, serialNumber, search, status);
            cmd.CommandText = querry;
            cmd.CommandType = CommandType.Text;

            return (BindData(cmd.ExecuteReader()));

        }

          public void bindParams(MySqlCommand cmd, int userId , string serialNumber , string search , string status)
        { 
            cmd.Parameters.Add(new MySqlParameter("find", search));
            cmd.Parameters.Add(new MySqlParameter("status", status));
            cmd.Parameters.Add(new MySqlParameter("userid", userId));
            cmd.Parameters.Add(new MySqlParameter("serialNumber", serialNumber));

        }



        public List<FaultyDeviceModel> BindData(DbDataReader reader)
        {
            var posts = new List<FaultyDeviceModel>();   // create an array of blogpost
            using (reader)
            {
                while (reader.Read())
                {
                    posts.Add(new FaultyDeviceModel()
                    {
                        complaintId = reader.GetInt32(reader.GetOrdinal("complaint_id")),
                        userId = reader.GetInt32(reader.GetOrdinal("employee_id")),
                        deviceId = reader.GetInt32(reader.GetOrdinal("device_id")),
                        salutation = reader.GetString("salutation"),
                        userName = new name()
                        {
                            first_name = reader.GetString("first_name"),
                            middle_name = new Specification().GetSafeString(reader, "middle_name"),
                            last_name = reader.GetString("last_name"),
                        },
                        serialNumber = reader.GetString("serial_number"),
                        deviceType = reader.GetString("type"),
                        deviceBrand = reader.GetString("brand"),
                        status = reader.GetString("status_name"),
                        deviceModel = reader.GetString("model"),
                        complaintDate = (reader.GetDateTime("complaint_date").ToString("dd-MM-yyyy")),
                        Issue = reader.GetString("comments")

                    }
            ); ;

                }
            }
            return posts;
        }



    }
}
