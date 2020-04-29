using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using dm_backend;
using System.Data.Common;

namespace dm_backend.Models
{
public class PartialDeviceModel
{
        public int device_id { get; set; }
        public int status_id { get; set; }
        public int specification_id { get; set; }
        public string type {get;set;}
        public string brand {get;set;}
        public string model { get; set; }
        public string color { get; set; }
        public string price { get; set; }
        public string serial_number { get; set; }
        public string warranty_year { get; set; }
        public string purchase_date { get; set; }
       
         internal AppDb Db { get; set; }

        public PartialDeviceModel()
        {
        }

        internal PartialDeviceModel(AppDb db)
        {
            Db = db;
        }
       public static string GetSafeString(MySqlDataReader reader, string colName)
        {

            return reader[colName] != DBNull.Value ? reader[colName].ToString() : "";
        }
        public static int GetInt(MySqlDataReader reader, string colName)
        {
            return reader[colName] != DBNull.Value ? (int)reader[colName] : default;
        }

       
}

    public class DeviceInsertUpdate:PartialDeviceModel
    {
        public string entry_date { get; set; }
       //  public AppDb Db { get; }
        public DeviceInsertUpdate(AppDb db)
        {
            Db = db;
        }

        public DeviceInsertUpdate()
        {
        }

        async public Task addDevice(DeviceInsertUpdate v)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = "insertdevice";
            cmd.CommandType = CommandType.StoredProcedure;
            BindDevice(cmd, v);
            await cmd.ExecuteNonQueryAsync();
        }
        private void BindDevice(MySqlCommand cmd, DeviceInsertUpdate v)
        {
            cmd.Parameters.Add(new MySqlParameter("device_type", v.type));
            cmd.Parameters.Add(new MySqlParameter("device_brand", v.brand));
            cmd.Parameters.Add(new MySqlParameter("device_model", v.model));
            cmd.Parameters.Add(new MySqlParameter("color", v.color));
            cmd.Parameters.Add(new MySqlParameter("price", v.price));
            cmd.Parameters.Add(new MySqlParameter("serial_number", v.serial_number));
            cmd.Parameters.Add(new MySqlParameter("warranty_year", v.warranty_year));
            cmd.Parameters.Add(new MySqlParameter("purchase_date", v.purchase_date));
            cmd.Parameters.Add(new MySqlParameter("status_id", v.status_id));
            cmd.Parameters.Add(new MySqlParameter("specification_id", v.specification_id));
            cmd.Parameters.Add(new MySqlParameter("entry_date", v.entry_date));
        }
        async public Task updateDevice(DeviceInsertUpdate v)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = "updatedevice";
            cmd.CommandType = CommandType.StoredProcedure;
            BindDeviceId(cmd, v);
            BindDevice(cmd, v);
            await cmd.ExecuteNonQueryAsync();
        }
        private void BindDeviceId(MySqlCommand cmd, DeviceInsertUpdate v)
        {
            cmd.Parameters.Add(new MySqlParameter("device_id", v.device_id));
        }
         public List<DeviceInsertUpdate> getdevicebyid(int device_id)
        {

            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = "getdevicebyid";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id",
                DbType = DbType.Int32,
                Value = device_id,
            });
            return Read(cmd.ExecuteReader());
        }
        private List<DeviceInsertUpdate> Read(MySqlDataReader reader)
        {
            var posts = new List<DeviceInsertUpdate>();
            using (reader)
            {
                while (reader.Read())
                {
                    var post = new DeviceInsertUpdate();
                    post.device_id = GetInt(reader, "device_id");
                    post.type = GetSafeString(reader, "type");
                    post.brand = GetSafeString(reader, "brand");
                    post.model = GetSafeString(reader, "model");
                    post.color = GetSafeString(reader, "color");
                    post.price = GetSafeString(reader, "price");
                    post.serial_number = GetSafeString(reader, "serial_number");
                    post.warranty_year = GetSafeString(reader, "warranty_year");
                    post.status_id = GetInt(reader, "status_id");
                    post.specification_id = GetInt(reader, "specification_id");
                    post.purchase_date = Convert.ToDateTime(reader["purchase_date"]).ToString("dd/MM/yyyy");
                    post.entry_date = Convert.ToDateTime(reader["entry_date"]).ToString("dd/MM/yyyy");
                    posts.Add(post);
                }
            }
            return posts;
        }

    }
    public class Assign:devices
    {
        public int user_id { get; set; }
        public int admin_id{get;set;}
       
        public Assign()
        {

        }

        internal Assign(AppDb db)
        {
            Db = db;
        }



        async public Task assignDevice(Assign d)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = "admin_assign_device";
            cmd.CommandType = CommandType.StoredProcedure;
            BindAssignDevice(cmd, d);
            await cmd.ExecuteNonQueryAsync();
        }
        private void BindAssignDevice(MySqlCommand cmd, Assign d)
        {
            cmd.Parameters.Add(new MySqlParameter("device_id", d.device_id));
            cmd.Parameters.Add(new MySqlParameter("return_date", d.return_date));
            cmd.Parameters.Add(new MySqlParameter("user_id", d.user_id));
            cmd.Parameters.Add(new MySqlParameter("admin_id",d.admin_id));

        }
    }

    public class devices:PartialDeviceModel
    {
        public string status { get; set; }
        public string comments { get; set; }
        public Specification specifications { get; set; }
        public string assign_date { get; set; }
        public string entry_date{get; set;}
        public string return_date { get; set; }
        public name assign_to { get; set; }
        public name assign_by { get; set; }

       

        public devices()
        {
            specifications = new Specification();
            assign_to = new name();
            assign_by = new name();
        }

        internal devices(AppDb db)
        {
            Db = db;
        }
        public Specification ReadSpecification(MySqlDataReader reader)
        {
            var spec1 = new Specification();
            spec1.RAM = GetSafeString(reader, "RAM");
            spec1.Storage = GetSafeString(reader, "Storage");
            spec1.ScreenSize = GetSafeString(reader, "Screen_size");
            spec1.Connectivity = GetSafeString(reader, "Connectivity");

            return spec1;
        }
        private name ReadName(MySqlDataReader reader, name name1, string prefix)
        {
            name1 = new name();
            name1.first_name = GetSafeString(reader, prefix + "_first_name");
            name1.middle_name = GetSafeString(reader, prefix + "_middle_name");
            name1.last_name = GetSafeString(reader, prefix + "_last_name");
            return name1;
        }

        public List<devices> SortAlldevices(String SortColumn, String SortDirection)
        {
            using var cmd = Db.Connection.CreateCommand();
        
            cmd.CommandText = "select ad.assign_date as assign_date,ad.return_date as return_date, " +
                "u1.first_name as assign_by_first_name,u1.middle_name as assign_by_middle_name," +
                "u1.last_name as assign_by_last_name,u.first_name as assign_to_first_name, " +
                "u.middle_name as assign_to_middle_name, u.last_name as assign_to_last_name, " +
                "d.device_id as device_id, dm.model as model, d.color as color,d.price as price," +
                " d.serial_number as serial_number, d.purchase_date as purchase_date," +
                " d.entry_date as entry_date, d.warranty_year as warranty_year, sf.RAM as RAM," +
                " sf.connectivity as connectivity, sf.storage as storage, sf.screen_size as screen_size," +
                "s.status_name, dt.type as type, db.brand as brand from device as d" +
                " inner join device_type as dt inner join device_model as dm inner join device_brand as db inner join status as s " +
                "inner join  specification as sf on d.device_type_id = dt.device_type_id and" +
                " d.device_brand_id = db.device_brand_id and d.device_model_id = dm.device_model_id and d.status_id = s.status_id and" +
                " d.specification_id = sf.specification_id left join assign_device as ad" +
                " on d.device_id = ad.device_id left join  user as u  on ad.user_id = u.user_id" +
                " left join user as u1 " +
                "on  u1.user_id = ad.assigned_by order by " + SortColumn + " " + SortDirection + ";";
            // BindColumn(cmd,SortColumn);
            return ReadAll(cmd.ExecuteReader());
        }


        public List<devices> GetAllDevices()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = "call getAllDevice";
            //cmd.Parameters.AddWithValue("@limit1", limit1);
            //cmd.Parameters.AddWithValue("@offset1", offset1);
            return ReadAll(cmd.ExecuteReader());

        }
        public List<devices> getDeviceBySearch(string device_name, string serial_number, string status_name)
        {
            using (var cmd = Db.Connection.CreateCommand())
            {

                cmd.CommandText = "call getDevicesBySearch(@device_name,@serial_number,@status_name)";
                cmd.Parameters.AddWithValue("@device_name", device_name);
                cmd.Parameters.AddWithValue("@serial_number", serial_number);
                cmd.Parameters.AddWithValue("@status_name", status_name);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                    return ReadAll(reader);
            }
        }
        // device description separate page
        public devices getDeviceDescriptionbyid(int device_id)
        {
            using var cmd = Db.Connection.CreateCommand();;
            cmd.CommandText = @"call device_description_byid(@id)";
            //cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@id",device_id);
            return ReadAllDeets(cmd.ExecuteReader())[0];
        }

        //delete device
        public int Delete()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = "delbyid";
            cmd.CommandType = CommandType.StoredProcedure;
            Bindid(cmd);
            cmd.ExecuteNonQuery();
            return 1;
        }
        private void Bindid(MySqlCommand cmd)
        {
            var device_idParam = new MySqlParameter("device_id", device_id);
            if (cmd.Parameters.Contains("device_id"))
            {
                cmd.Parameters["device_id"].Value = device_id;
            }
            else
            {
                cmd.Parameters.Add(device_idParam);
            }
        }

        private List<devices> ReadAll(MySqlDataReader reader)
        {
            var posts = new List<devices>();
            using (reader)
            {
                while (reader.Read())
                {
                    var post = new devices();
                    post.device_id = GetInt(reader, "device_id");
                    post.type = GetSafeString(reader, "type");
                    post.brand = GetSafeString(reader, "brand");
                    post.model = GetSafeString(reader, "model");
                    post.color = GetSafeString(reader, "color");
                    post.price = GetSafeString(reader, "price");
                    post.serial_number = GetSafeString(reader, "serial_number");
                    post.warranty_year = GetSafeString(reader, "warranty_year");
                    post.status = GetSafeString(reader, "status_name");
                    post.purchase_date = Convert.ToDateTime(reader["purchase_date"]).ToString("dd/MM/yyyy");
                    post.specifications = ReadSpecification(reader);
                    //post.comments = GetSafeString(reader, "comments");
                    post.assign_date = GetSafeString(reader, "assign_date");
                    post.return_date = GetSafeString(reader, "return_date");
                    post.assign_by = ReadName(reader, post.assign_by, "assign_by");
                    post.assign_to = ReadName(reader, post.assign_to, "assign_to");
                    posts.Add(post);
                }
            }
            return posts;
        }
        private List<devices> ReadAllDeets(MySqlDataReader reader)
        {
            var posts = new List<devices>();
            using (reader)
            {
                while (reader.Read())
                {
                    var post = new devices();
                    post.device_id = GetInt(reader, "device_id");
                    post.type = GetSafeString(reader, "type");
                    post.brand = GetSafeString(reader, "brand");
                    post.model = GetSafeString(reader, "model");
                    post.color = GetSafeString(reader, "color");
                    post.price = GetSafeString(reader, "price");
                    post.serial_number = GetSafeString(reader, "serial_number");
                    post.warranty_year = GetSafeString(reader, "warranty_year");
                    post.status = GetSafeString(reader, "status_name");
                    post.purchase_date = Convert.ToDateTime(reader["purchase_date"]).ToString("dd/MM/yyyy");
                    post.entry_date = Convert.ToDateTime(reader["entry_date"]).ToString("dd/MM/yyyy");
                    post.specifications = ReadSpecification(reader);
                    post.comments = GetSafeString(reader, "comments");
                    posts.Add(post);
                    }
            }
            return posts;
        }

       
        public List<devices> getCurrentDevice(int id, string search, string sort = "", string direction = "")
        {
            using var cmd = Db.Connection.CreateCommand();

            cmd.CommandText = @"select * from(select device_type.type,device_brand.brand,device_model.model,assign_device.assign_date,assign_device.return_date,assign_device.device_id from user,device_type,device_model,device_brand,assign_device,device,status
where  user.user_id=assign_device.user_id and assign_device.device_id=device.device_id and device.device_type_id=device_type.device_type_id and device.device_brand_id=device_brand.device_brand_id
and device.device_model_id=device_model.device_model_id  and assign_device.status_id=status.status_id and status.status_name='Allocated' and assign_device.user_id=" + @id + ") as demo WHERE demo.type LIKE '%" + @search + "%' or demo.brand LIKE '%" + @search + "%' or demo.model LIKE '%" + @search + "%' ";

            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id",
                DbType = DbType.Int32,
                Value = id,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@search",
                DbType = DbType.String,
                Value = search,
            });
            if (!string.IsNullOrEmpty(sort) && !string.IsNullOrEmpty(direction))
            {


                cmd.CommandText += "order by " + sort + " " + direction + "";

            }
            Console.WriteLine(cmd.CommandText);
            Console.WriteLine("id = " + cmd.Parameters["@id"].Value);
            Console.WriteLine("Search = " + cmd.Parameters["@search"].Value);
            // Console.WriteLine("Search = " + cmd.Parameters["@sort"].Value);

            return  ReadAllDevice(cmd.ExecuteReader());

        }
        public List<devices> getPreviousDevice(int id, string search = "", string sort = "", string direction = "")
        {
            using var cmd = Db.Connection.CreateCommand();

            cmd.CommandText = @"select * from(select device_type.type,device_brand.brand,device_model.model,assign_date,return_date,request_history.device_id from user,device_type,device_brand,device_model,request_history 
where user.user_id=request_history.user_id and request_history.device_type=device_type.device_type_id and request_history.device_brand=device_brand.device_brand_id 
and request_history.device_model=device_model.device_model_id and request_history.user_id=" + @id + ") as demo WHERE demo.type LIKE '%" + @search + "%' or demo.brand LIKE '%" + @search + "%' or demo.model LIKE '%" + @search + "%' ";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id",
                DbType = DbType.Int32,
                Value = id,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@search",
                DbType = DbType.String,
                Value = search,
            });
            if (!string.IsNullOrEmpty(sort) && !string.IsNullOrEmpty(direction))
            {


                cmd.CommandText += "order by " + sort + " " + direction + "";

            }
            Console.WriteLine(cmd.CommandText);
            Console.WriteLine("id = " + cmd.Parameters["@id"].Value);
            Console.WriteLine("Search = " + cmd.Parameters["@search"].Value);
            // Console.WriteLine("Search = " + cmd.Parameters["@sort"].Value);
            return  ReadAllDevice(cmd.ExecuteReader());




        }
        
	
		public List<devices> ReadAllDevice(MySqlDataReader reader)
		{
			Console.WriteLine("Rows" + reader.HasRows);
			var posts = new List<devices>();
			using (reader)
			{
				while (reader.Read())
				{
					Console.WriteLine("Found a row");
					var post = new devices()
					{
						type = reader.GetString(0),
						brand = reader.GetString(1),
						model = reader.GetString(2),
						assign_date = GetSafeString(reader,"assign_date"),
						return_date = GetSafeString(reader,"return_date"),
                        device_id = GetInt(reader,"device_id"),
                       // user_id =GetInt(reader,"user_id")
					};
					posts.Add(post);
				}
			}
			return posts;
		}

    }



}
