using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using dm_backend.Models;
using System.Data.Common;

namespace dm_backend.Utilities
{
    public static class Readers
    {

        public static string GetSafeString(MySqlDataReader reader, string colName)
        {
            return reader[colName] != DBNull.Value ? (string)reader[colName] : "";
        }
        public static DateTime GetSafeDate(MySqlDataReader reader, string colName)
        {
            return reader[colName] != DBNull.Value ? Convert.ToDateTime(reader[colName]) : DateTime.MinValue;
        }
        public static T GetSafeInt<T>(MySqlDataReader reader, string colName)
        {
            return reader[colName] != DBNull.Value ? (T)reader[colName] : default(T);
        }
         public static T GetSafe<T>(DbDataReader reader, string colName)
        {
            return reader[colName] != DBNull.Value ? (T)reader[colName] : default(T);
        }
        public static PartialUserModel ReadPartialUser(MySqlDataReader reader)
        {
            PartialUserModel user = new User();
            user.Salutation = GetSafeString(reader, "salutation");
            user.FirstName = GetSafeString(reader, "first_name");
            user.MiddleName = GetSafeString(reader, "middle_name");
            user.LastName = GetSafeString(reader, "last_name");
            user.DepartmentName = GetSafeString(reader, "department_name");
            user.DesignationName = GetSafeString(reader, "designation_name");
            user.Email = GetSafeString(reader, "email");
            user.Gender = GetSafeString(reader, "gender");
            user.DOB = GetSafeDate(reader,"date_of_birth").ToString("yyyy-MM-dd");
            user.DOJ = GetSafeDate(reader,"date_of_joining").ToString("yyyy-MM-dd");
            return user;
        }
        public static Specification ReadSpecifications(MySqlDataReader reader)
        {
            var specfication = new Specification();
            specfication.RAM = (string)reader["RAM"];
            specfication.Storage = (string)reader["storage"];
            specfication.ScreenSize = (string)reader["screen_size"];
            specfication.Connectivity = (string)reader["connectivity"];
            return specfication;
        }
        public static RequestModel ReadRequest(MySqlDataReader reader){
            var request = new RequestModel();
            request.requestId = GetSafeInt<int>(reader, "request_device_id");
            request.userId = (int)reader["user_id"];
            request.deviceModel = (string)reader["model"];
            request.deviceBrand = (string)reader["brand"];
            request.deviceType = (string)reader["type"];
            request.requestDate = Convert.ToDateTime(reader["request_date"]).ToString("yyyy-MM-dd");
            request.noOfDays = (int)reader["no_of_days"];  // To convert sbyte to int
            request.comment = GetSafeString(reader, "comment");
            request.availability = GetSafeInt<Int32>(reader, "availability") == 1 ? true : false;
            return request;
        }
        //  public static NotificationModel ReadNotification(MySqlDataReader reader){
        //     var notify = new NotificationModel();
        //     notify.notificationId = GetSafeInt<int>(reader, "notification_id");
        //     notify.userId = (int)reader["user_id"];
        //     notify.deviceId =(int)reader["device_id"];
        //     notify.deviceModel = (string)reader["model"];
        //     notify.deviceBrand = (string)reader["brand"];
        //     notify.deviceType = (string)reader["type"];
        //     // notify.notificationDate = Convert.ToDateTime(reader["notification_date"]).ToString("yyyy-MM-dd");
        //     notify.notificationDate = (string)reader["notification_date"];
        //     notify.message = GetSafeString(reader, "message");
        //     notify.status = (string)reader["status_name"];
        //     return notify;
        // }
        public static ReturnRequestModel ReadReturnRequests(MySqlDataReader reader){
            var request = new ReturnRequestModel();
            request.returnRequestId = GetSafeInt<int>(reader, "return_request_id");
            request.userId = (int)reader["user_id"];
            request.salutation = (string)reader["salutation"];
            request.firstName = (string)reader["first_name"];
            request.middleName = GetSafeString(reader, "middle_name");
            request.lastName   = (string)reader["last_name"];
            request.deviceId = (int)reader["device_id"];
            request.deviceModel = (string)reader["model"];
            request.deviceBrand = (string)reader["brand"];
            request.deviceType = (string)reader["type"];
            request.returnDate = Convert.ToDateTime(reader["return_date"]).ToString("yyyy-MM-dd");
            return request;
        }
    }
}
