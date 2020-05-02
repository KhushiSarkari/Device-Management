using System.Collections.Generic;
using System.Threading.Tasks;
using dm_backend.Models;
using System.Data.Common;
using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace dm_backend.Models{
    public class BindRequestData
    {
        public AppDb Db { get; }

        public BindRequestData(AppDb db)
        {
            Db = db;
        }
               
        public BindRequestData()

        {

        }
        async public Task<List<RequestDeviceHistory>> BindHistoryData(DbDataReader reader)
        {
            var posts = new List<RequestDeviceHistory>();   // create an array of blogpost
            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    posts.Add(new RequestDeviceHistory()
                    {
                        RequestedUser = FillUserName(reader),
                        UserMail = reader.GetString("email"),
                        DeviceID = (reader.IsDBNull("device_id")) ? -1 : (int)reader["device_id"],
                        DeviceType = reader.GetString("type"),
                        DeviceBrand = reader.GetString("brand"),
                        DeviceModel = reader.GetString("model"),
                        Specs = FillDeviceSpecifications(reader),
                        RequestStatus = reader.GetString("status_name"),
                        Serial_number = reader.IsDBNull("serial_number") ? "" : reader.GetString("serial_number"),
                        DeviceSubmittedAdmin = FillAdminName(reader),
                        AssignedDate = reader.IsDBNull("assign_date") ?" " : reader.GetDateTime("assign_date").ToString("dd-MM-yyyy"),
                        RequestDate = (reader.IsDBNull("request_date")) ? " " : reader.GetDateTime("request_date").ToString("dd-MM-yyyy"),
                        ReturnDate = (reader.IsDBNull("return_date")) ? " " : reader.GetDateTime("return_date").ToString("dd-MM-yyyy"),
                        AssignDays = reader.IsDBNull("assign_days") ? 0 : (int)(sbyte)reader["assign_days"],
                    }
            );
                    
                }
            }
            return posts;
        }

        public Specification FillDeviceSpecifications(DbDataReader reader)
        {
            return new Specification()
            {
                specification_id = reader.IsDBNull("specification_id") ? -1 : (int)reader["specification_id"],
                RAM = reader.IsDBNull("RAM") ? "" : reader.GetString("RAM"),
                Storage = reader.IsDBNull("storage") ? "" : reader.GetString("storage"),
                ScreenSize = reader.IsDBNull("screen_size")  ? "" : reader.GetString("screen_size"),
                Connectivity = reader.IsDBNull("connectivity")  ? "" : reader.GetString("connectivity")

            };

        }
        public UserName FillUserName(DbDataReader reader)
        {
           
            return new UserName()
            {

                userId = reader.IsDBNull("user_id") ? -1 : reader.GetInt32(reader.GetOrdinal("user_id")),
                salutation =  reader.IsDBNull("salutation") ? "" : reader.GetString("salutation"),
                firstName = reader.IsDBNull("first_name") ? "" : reader.GetString("first_name"),
                middleName = reader.IsDBNull("middle_name") ? "" : reader.GetString("middle_name"),
                Lastname = reader.IsDBNull("Last_name") ? "" : reader.GetString("Last_name")
            };

        }
        public UserName FillAdminName(DbDataReader reader)
        {
           
            return new UserName()
            {

                userId = reader.IsDBNull("admin_user_id") ? -1 : reader.GetInt32(reader.GetOrdinal("admin_user_id")),
                salutation = reader.IsDBNull("admin_salutation") ? "" : reader.GetString("admin_salutation"),
                firstName = reader.IsDBNull("admin_first_name") ? "" : reader.GetString("admin_first_name"),
                middleName = reader.IsDBNull("admin_middle_name") ? "" : reader.GetString("admin_middle_name"),
                Lastname = reader.IsDBNull("admin_last_name") ? "" : reader.GetString("admin_last_name")
            };

        }




    }

}
