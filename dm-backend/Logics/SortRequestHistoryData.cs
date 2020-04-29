using MySql.Data.MySqlClient;
using dm_backend.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;


namespace dm_backend.Logics
{
    public class SortRequestHistoryData
    {

        public AppDb Db { get; }


        private string command = @"select u.user_id ,  sl.salutation , u.first_name , u.middle_name , u.last_name , u.email , rh.device_id , d.serial_number as serial_number ,  
            dt.type , db.brand , dm.model , sp.*, rh.assign_date ,rh.assign_days , rh.return_date , rh.request_date  ,  s.status_name , rh.return_to as admin_user_id  , 
             s1.salutation as  admin_salutation  ,  u1.first_name as admin_first_Name , u1.middle_name as admin_middle_name , u1.last_name as  admin_last_name  from request_history as rh
            inner join status as s  inner join specification as sp  inner join user as u   inner join device_type as dt inner join device_brand as db
              inner join salutation as sl  inner join device_model as  dm on  dm.device_model_id= rh.device_model and db.device_brand_id = rh.device_brand and
            sl.salutation_id = u.salutation_id and  dt.device_type_id = rh.device_type and  sp.specification_id =  rh.specification_id and
             s.status_id =  rh.status_id and  u.user_id = rh.user_id left join user u1  on  rh.return_to = u1.user_id  left join salutation as s1 on
            s1.salutation_id = u1.salutation_id  left join device as d  on d.device_id  = rh.device_id 
            where  concat(u.first_name , ' ', if (u.middle_name is null, '' ,  concat(u.middle_name, ' ')) , u.last_name ) like concat('%' ,@find ,'%') 
             and  if(@status is null ,  s.status_name like '%' OR s.status_name is null  , s.status_name = @status)  and if(@serialNumber is null , d.serial_number like '%'  OR d.serial_number is null ,  d.serial_number = @serialNumber )";

        public SortRequestHistoryData(AppDb db)
        {
            Db = db;
        }

        public void FindSortingAttribute(string value)
        {

            value = value.ToLower();
            var attribute = value switch
            {
                "user" =>
                        "order by concat(u.first_name , ' ', if (u.middle_name is null, '' , concat(u.middle_name , ' ')) , u.last_name )",

                "admin" =>
                        "order by  concat(u1.first_name , ' ', if (u1.middle_name is null, '' , concat(u1.middle_name , ' ')) , u1.last_name )",

                "serialnumber" =>
                         "order by d.serial_number",

                "device" =>
                         "order by concat(db.brand, ' ', dm.model)",

                _ =>
                    "order by concat(u.first_name , ' ', if (u.middle_name is null, '' , concat(u.middle_name , ' ')) , u.last_name )"
            };

            this.command += " " + attribute;
            
        }




        private int page_limit(string limit , int def)
        {
            int limit_size;
                try
            {
                if(limit != "" &&  Int32.Parse(limit) > 0)
                limit_size = Int32.Parse(limit);
                else
                    limit_size = def;
            }
            catch(Exception e)
            {
                limit_size = def;
            }
            return limit_size;
        }

   


        async public Task<Result<RequestDeviceHistory>> GetSortData(string find, string serialNumber, string status , string sortElement, string sortType, string page, string limit)
        {
           
            using var cmd = Db.Connection.CreateCommand();
      
            FindSortingAttribute(sortElement);
            this.command += " " + sortType;
            int pageValue = page_limit(page, 1);
            int limitValue = page_limit(limit, 10);
            int offset = ((pageValue-1) * limitValue);

            cmd.CommandText = command + " limit @offset , @limit ;";
            
            cmd.CommandType = CommandType.Text;

            BindLimitParams(cmd, offset, limitValue );
            BindSearchParms(cmd, find , serialNumber , status );

            try
            {

                var data = await new BindRequestData(Db).BindHistoryData(await cmd.ExecuteReaderAsync());

                return await new TotalResultCount(Db).FindCount(data, command, find, offset, limitValue, serialNumber , status);

            }
            catch (Exception e)
            {
                throw e;
            }
        }



        public void BindSearchParms(MySqlCommand cmd, string find , string serialNumber , string status)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@find",
                DbType = DbType.String,
                Value = find,
            });
            cmd.Parameters.Add(new MySqlParameter("@serialnumber", serialNumber));
            cmd.Parameters.Add(new MySqlParameter("@status", status));
           
        } 
            public void BindLimitParams(MySqlCommand cmd, int low , int high  )
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@offset",
                DbType = DbType.Int32,
                Value = low,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@limit",
                DbType = DbType.Int32,
                Value = high,
            });
        }

        


    }
}
