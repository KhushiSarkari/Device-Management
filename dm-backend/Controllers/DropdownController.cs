using System;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using dm_backend.Models;
using Microsoft.AspNetCore.Authorization;

namespace dm_backend.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class DropdownController : ControllerBase
    {
        public DropdownController(AppDb db)
        {
            Db = db;
        }
        [AllowAnonymous]
        [HttpGet]
        [Route("country")]
        public IActionResult Countries()
        {

            List<DropdownModel> countries = new List<DropdownModel>();
            Db.Connection.Open();
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = "select country_id, country_name from country";
            var reader = cmd.ExecuteReader();

            using (reader)
            {
                while (reader.Read())
                {
                    countries.Add(new DropdownModel()
                    {
                        id = reader.GetInt32(0),
                        name = reader.GetString(1)
                    });
                }
            }
            Db.Connection.Close();
            if (countries.Count > 0)
            {
                return Ok(countries);
            }
            else
                return NoContent();
        }
         [AllowAnonymous]

        [HttpGet]
        [Route("state")]
        public IActionResult States()
        {
            String fields = HttpContext.Request.Query["id"];
            List<DropdownModel> states = new List<DropdownModel>();
            Db.Connection.Open();
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = "select state_id, state_name from state";
            if(!string.IsNullOrEmpty(fields)){
            cmd.CommandText+= " inner join country using(country_id) where country_id=@c_id";
            cmd.Parameters.AddWithValue("@c_id", fields);
            }
            cmd.CommandText+=" order by state_name asc";
           
            var reader = cmd.ExecuteReader();

            using (reader)
            {
                while (reader.Read())
                {
                    states.Add(new DropdownModel()
                    {
                        id = reader.GetInt32(0),
                        name = reader.GetString(1)
                    });
                }
            }
            Db.Connection.Close();
            if (states.Count > 0)
            {
                return Ok(states);
            }
            else
                return NoContent();
        }
         [AllowAnonymous]

        [HttpGet]
        [Route("city")]
        public IActionResult Cities()
        {
            String fields = HttpContext.Request.Query["id"];
            List<DropdownModel> cities = new List<DropdownModel>();
            Db.Connection.Open();
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = "select city_id, city_name from city";
            if(!string.IsNullOrEmpty(fields)){
                cmd.CommandText+=" inner join state using(state_id) where state_id=@c_id";
            cmd.Parameters.AddWithValue("@c_id", fields);
            } 
             cmd.CommandText+=" order by city_name asc";
            var reader = cmd.ExecuteReader();

            using (reader)
            {
                while (reader.Read())
                {
                    cities.Add(new DropdownModel()
                    {
                        id = reader.GetInt32(0),
                        name = reader.GetString(1)
                    });
                }
            }
            Db.Connection.Close();
            if (cities.Count > 0)
            {
                return Ok(cities);
            }
            else
                return NoContent();
        }




        [HttpGet]
        [Route("designation")]
        public IActionResult designationTypes()
        {
            String fields = HttpContext.Request.Query["id"];
            List<DropdownModel> desgTypes = new List<DropdownModel>();
            Db.Connection.Open();
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = "select designation.designation_id,designation.designation_name from department,designation,department_designation where department.department_id=department_designation.department_id and designation.designation_id=department_designation.designation_id and department.department_name=@fields;";
             cmd.Parameters.AddWithValue("@fields", fields);
           
            var reader = cmd.ExecuteReader();

            using (reader)
            {
                while (reader.Read())
                {
                    desgTypes.Add(new DropdownModel()
                    {
                        id = reader.GetInt32(0),
                        name = reader.GetString(1)
                    });
                }
            }
            Db.Connection.Close();
            if (desgTypes.Count > 0)
            {
                return Ok(desgTypes);
            }
            else
                return NoContent();
        }



        [HttpGet]
        [Route("department")]
        public IActionResult departmentTypes()
        {
          
            List<DropdownModel> deptTypes = new List<DropdownModel>();
            Db.Connection.Open();
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = "select * from department";
           
            var reader = cmd.ExecuteReader();

            using (reader)
            {
                while (reader.Read())
                {
                    deptTypes.Add(new DropdownModel()
                    {
                        id = reader.GetInt32(0),
                        name = reader.GetString(1)
                    });
                }
            }
            Db.Connection.Close();
            if (deptTypes.Count > 0)
            {
                return Ok(deptTypes);
            }
            else
                return NoContent();
        }

        [Route("salutation")]
        public IActionResult Salutations()
        {
            var result = GetListFromQuery("select * from salutation;");
            if (result.Count < 1)
                return NoContent();
            return Ok(result);

        }
        [HttpGet]
        [Route("country_code")]
        public IActionResult CountryCodes()
        {

            List<DropdownModel> countries = new List<DropdownModel>();
            Db.Connection.Open();
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = "select country_code, country_name from country";
            var reader = cmd.ExecuteReader();

            using (reader)
            {
                while (reader.Read())
                {
                    countries.Add(new DropdownModel()
                    {
                        id = Convert.ToInt32(reader.GetString(0)),
                        name = reader.GetString(1),

                    });
                }
            }
            Db.Connection.Close();
            if (countries.Count > 0)
            {
                return Ok(countries);
            }
            else
                return NoContent();
        }
     [Route("addressType")]
        public IActionResult addressTypes()
        {
            var result = GetListFromQuery("select * from address_type;");
            if (result.Count < 1)
                return NoContent();
            return Ok(result);
            
        }

        [Route("contactType")]
        public IActionResult contactTypes()
        {
             var result = GetListFromQueryWithId("select * from contact_type;");
            if (result.Count < 1)
                return NoContent();
            return Ok(result);


            
        }


        [HttpGet]
        [Route("{type}/{brand}/{model}/specification")]
        public  IActionResult GetAllDeviceBrands(String type ,String brand ,  String model )
        {

            Db.Connection.Open();

            var specs = new Specification(Db);

            var result = specs.getSpecificSpecification( type, brand, model);

            Db.Connection.Close();
             if (result.Count < 1)
               return NoContent();
            //var json = config.Formatters.JsonFormatter;
           // json.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.None;
            return new OkObjectResult(result);
        }


        [HttpGet]   
        [Route("brands")]
        public IActionResult GetAllDeviceBrands()
        {
            var result = GetListFromQuery("select brand from device_brand");
            if (result.Count < 1)
                return NoContent();
            return Ok(result);
        }



        [HttpGet]
        [Route("brands/{type}")]
        public IActionResult BrandsOfType(string type)
        {
            var querry = @"select distinct( db.brand )from device_brand as  db inner join device   using (device_brand_id) inner join device_type as dt
using (device_type_id) where  dt.type = '" + type + "' ;";
            var result = GetListFromQuery(querry);
            if (result.Count < 1)
                return NoContent();
            return Ok(result);
        }

        [HttpGet]
        [Route("models")]
        public IActionResult GetAllDeviceModels()
        {
            var result = GetListFromQuery("select model from device_model");
            if (result.Count < 1)
                return NoContent();
            return Ok(result);
        }


        [HttpGet]
        [Route("models/{brand}")]
        public IActionResult ModelByBrand( string brand)
        {
            var querry = @"select distinct( dm.model )from device_model as  dm inner join device   using (device_model_id) inner join device_brand as db
using (device_brand_id) where  db.brand = '" + brand + "' ;";
            var result = GetListFromQuery(querry);
            if (result.Count < 1)
                return NoContent();
            return Ok(result);
        }

        
        
        [HttpGet]
        [Route("types")]
        public IActionResult GetAllDeviceTypes()
        {
            var result = GetListFromQuery("select type from device_type");
            if (result.Count < 1)
                return NoContent();
            return Ok(result);
        }

        [HttpGet]
        [Route("rams")]
        public IActionResult GetAllSpecificationRAM()
        {
            var result = GetListFromQuery("select distinct RAM from specification");
            if (result.Count < 1)
                return NoContent();
            return Ok(result);
        }

        [HttpGet]
        [Route("storages")]
        public IActionResult GetAllSpecificationStorage()
        {
            var result = GetListFromQuery("select distinct storage from specification");
            if (result.Count < 1)
                return NoContent();
            return Ok(result);
        }

        [HttpGet]
        [Route("screensizes")]
        public IActionResult GetAllSpecificationScreenSize()
        {
            var result = GetListFromQuery("select distinct screen_size from specification");
            if (result.Count < 1)
                return NoContent();
            return Ok(result);
        }

        [HttpGet]
        [Route("connectivities")]
        public IActionResult GetAllSpecificationConnectivity()
        {
            var result = GetListFromQuery("select distinct connectivity from specification");
            if (result.Count < 1)
                return NoContent();
            return Ok(result);
        }
          [HttpGet("status")]
        public IActionResult GetAllstatus()
        {
            var result = GetListFromQueryWithId("select * from status where status.status_name='Allocated' or status.status_name='Free' or status.status_name='Faulty';");
            if (result.Count < 1)
                return NoContent();
            return Ok(result);

        }
         [HttpGet]
         [Route("userlist")]
        public IActionResult GetUserDetails()
        {
            List<DropdownModel> userDetails = new List<DropdownModel>();
            Db.Connection.Open();
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = "select user.user_id,user.first_name,department.department_name from user,department,"+
            "department_designation,status where"+
            " user.department_designation_id=department_designation.department_designation_id and"+
            " department_designation.department_id=department.department_id and user.status=status.status_id"+
            " and status.status_name= 'Active'";
            var reader = cmd.ExecuteReader();
            using (reader)
            {
                while (reader.Read())
                {
                    userDetails.Add(new DropdownModel()
                    {
                        id = reader.GetInt32(0),
                        name = reader.GetString(1),
                        department = reader.GetString(2)
                    });
                }
            }
            Db.Connection.Close();
            if (userDetails.Count > 0)
            {
                return Ok(userDetails);
            }
            else
                return NoContent();
        }



        private List<DropdownModel> GetListFromQueryWithId(string queryString)
        {
            Db.Connection.Open();
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = queryString;
            var reader = cmd.ExecuteReader();
            var result = new DropdownModel().ReadAllDropdownswithId(reader);
            Db.Connection.Close();
            return result;
        }
       
        private List<DropdownModel> GetListFromQuery(string queryString)
        {
            Db.Connection.Open();
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = queryString;
            var reader = cmd.ExecuteReader();
            var result = new DropdownModel().ReadAllDropdowns(reader);
            Db.Connection.Close();
            return result;
        }
        
        public AppDb Db { get; }
    }

    class DropdownModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string department {get;set;}

        public List<DropdownModel> ReadAllDropdowns(MySqlDataReader reader)
        {
            var list = new List<DropdownModel>();
            while (reader.Read())
            {
                list.Add(new DropdownModel()
                {
                //    id = reader.GetInt32(0),
                    name = reader.GetString(0)
                });
            }
            return list;
        }
        public List<DropdownModel> ReadAllDropdownswithId(MySqlDataReader reader)
        {
            var list = new List<DropdownModel>();
            while (reader.Read())
            {
                list.Add(new DropdownModel()
                {
                   id = reader.GetInt32(0),
                    name = reader.GetString(1)
                });
            }
            return list;
        }
    }
}