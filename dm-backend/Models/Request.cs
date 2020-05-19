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
    public class RequestModel
    {
        public int? requestId { get; set; }
        public int userId { get; set; }
        public string deviceModel { get; set; }
        public string deviceBrand { get; set; }
        public string deviceType { get; set; }
        public Specifications specs { get; set; }
        public PartialUserModel requestedBy { get; set; }        
        public string requestDate { get; set; }
        public int noOfDays { get; set; }
        public string comment { get; set; }
        public bool? availability { get; set; }
        internal AppDb Db { get; set; }

        public RequestModel()
        {
        }
        public RequestModel(AppDb db)
        {
            Db = db;
        }

 
        public List<RequestModel> GetAllPendingRequests(int userId,string sortField,string sortDirection,string searchField)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = get_all_pending_requests+searchQuery;
            if(userId!=-1)
                cmd.CommandText +=" and user_id="+userId;
            cmd.CommandText +=" order by "+sortField+ " "+sortDirection; 
            cmd.Parameters.AddWithValue("@search_field", searchField); 
            // Console.WriteLine(cmd.CommandText);
            using MySqlDataReader reader =  cmd.ExecuteReader();
            return ReadAll(reader);
        }

        public void DeviceRequestAction(int id,string action){
            using var cmd = Db.Connection.CreateCommand();
            if(action=="accept")
                cmd.CommandText = "accept_request";
            else if(action=="reject")
                cmd.CommandText = "reject_request";
            cmd.CommandType = CommandType.StoredProcedure;
            try
            {
                BindId(cmd);
                cmd.Parameters.Add(new MySqlParameter("var_admin_id", id));
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        private void BindId(MySqlCommand cmd){
            cmd.Parameters.Add(new MySqlParameter("var_request_id", requestId));
        }

        private List<RequestModel> ReadAll(MySqlDataReader reader)
        {
            var requests = new List<RequestModel>();
            using (reader)
            {
                while (reader.Read())
                {
                    
                    var request = ReadRequest(reader);
                    request.specs = ReadSpecifications(reader);
                    request.requestedBy = ReadPartialUser(reader);
                    requests.Add(request);
                }
            }
            return requests;
        }

        public string CancelRequest(int  requestId)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = "cancel_request";
            cmd.CommandType = CommandType.StoredProcedure; 
            try{
                cmd.Parameters.AddWithValue("@request_id", requestId);
                cmd.ExecuteNonQuery();
                return "Request cancelled";
            }
            catch(Exception e){
                throw e;
            }
            
        }
    internal string get_all_pending_requests=@"select * from(select request_device_id, user_id, device_model.model, device_type.type, device_brand.brand, specification.*, request_date, no_of_days, comment, salutation, first_name, middle_name, last_name, department_name, designation_name, email, date_of_birth, date_of_joining, gender,
    if(count(available_devices.device_id) =1, TRUE, FALSE) as availability
    from request_device
    inner join user using(user_id)
    inner join department_designation using(department_designation_id)
    inner join department using(department_id)
    inner join designation using(designation_id)
    inner join device_brand using(device_brand_id)
    inner join device_model using(device_model_id)
    inner join device_type using(device_type_id)
    inner join specification using(specification_id)
    inner join salutation using(salutation_id)
    inner join gender using(gender_id)
    left join (
		select * from device
        inner join status
        using(status_id)
        where status_name='Free'
    ) as available_devices
    on available_devices.device_model_id = request_device.device_model_id
    and available_devices.specification_id = request_device.specification_id
    and available_devices.device_type_id = request_device.device_type_id
	and available_devices.device_brand_id = request_device.device_brand_id
    group by request_device_id) as demo";

    internal string searchQuery=@" having (type like CONCAT('%', @search_field, '%') or model like CONCAT('%', @search_field, '%') or brand like CONCAT('%', @search_field, '%') or get_full_name(user_id) like CONCAT('%', @search_field, '%')) ";       

    }   
}