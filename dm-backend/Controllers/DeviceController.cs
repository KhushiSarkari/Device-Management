using System;
using System.Threading.Tasks;
using dm_backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using dm_backend.Utilities;

namespace dm_backend.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class DeviceController : ControllerBase
    {
        public DeviceController(AppDb db)
        {
            Db = db;
        }

        [HttpGet]
        [Route("page")]
        public IActionResult GetAllDevices()
        {

            int pageNumber = Convert.ToInt32((string)HttpContext.Request.Query["page"]);
            int pageSize = Convert.ToInt32((string)HttpContext.Request.Query["page-size"]);
            Db.Connection.Open();
            var query = new devices(Db);
            var pager = PagedList<devices>.ToPagedList(query.GetAllDevices(),pageNumber, pageSize);
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(pager.getMetaData()));
            Db.Connection.Close();
            return Ok(pager);
        }
        [HttpGet]
        [Route("device_id/{device_id}")]
        public IActionResult GetOneDevice(string device_id)
        {
            Db.Connection.Open();
            var query = new DeviceInsertUpdate(Db);
            var result = query.getdevicebyid(Int32.Parse(device_id));
            Db.Connection.Close();
            return Ok(result);
        }

        [HttpGet]
        [Route("{device_id}")]
        public IActionResult GetDeviceFullDetails(string device_id)
        {
            Db.Connection.Open();
            var query = new devices(Db);
            var result = query.getDeviceDescriptionbyid(Int32.Parse(device_id));
            Db.Connection.Close();
            return Ok(result);
        }


        [HttpGet]
        [Route("search")]
        public IActionResult getDeviceswithSearch()
        {
            int pageNumber = Convert.ToInt32((string)HttpContext.Request.Query["page"]);
            int pageSize = Convert.ToInt32((string)HttpContext.Request.Query["page-size"]);
            string device_name = (HttpContext.Request.Query["device_name"]);
            string serial_number = (HttpContext.Request.Query["serial_number"]);
            string status_name = (HttpContext.Request.Query["status_name"]);
            if (device_name == null)
            {
                device_name = "";
            }
            Db.Connection.Open();
            var query = new devices(Db);
            var pager = PagedList<devices>.ToPagedList(query.getDeviceBySearch(device_name, serial_number, status_name), pageNumber, pageSize);
           Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(pager.getMetaData()));
            Db.Connection.Close();
            return Ok(pager);

        }
        [HttpGet]
        [Route("sort")]
        public IActionResult getDeviceswithSorting()
        {
            int pageNumber = Convert.ToInt32((string)HttpContext.Request.Query["page"]);
            int pageSize = Convert.ToInt32((string)HttpContext.Request.Query["page-size"]);
            string SortColumn = (HttpContext.Request.Query["SortColumn"]);
            string SortDirection = (HttpContext.Request.Query["SortDirection"]);
            SortDirection = (SortDirection.ToLower()) == "desc" ? "DESC" : "ASC";
            switch (SortColumn.ToLower())
            {
                case "device_name":
                    SortColumn = "concat(type ,'', brand , '' ,  model)";
                    break;
                case "specification":
                    SortColumn = "concat(RAM , ' ',storage ,' ',screen_size , ' ',connectivity)";
                    break;

                case "serial_number":
                    SortColumn = "serial_number*1";
                    break;

                default:
                    SortColumn = "concat(type ,'', brand , '' ,  model)";

                    break;
            }
            Db.Connection.Open();
            var query = new devices(Db);
            var pager = PagedList<devices>.ToPagedList(query.SortAlldevices(SortColumn, SortDirection), pageNumber, pageSize);
           Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(pager.getMetaData()));
            Db.Connection.Close();
            return Ok(pager);

        }

        [Authorize(Roles = "admin")]
        [HttpDelete]
        [Route("del/{device_id}")]
        public IActionResult DeleteOne(int device_id)
        {
            Db.Connection.Open();
            devices query = new devices(Db);
            query.device_id = device_id;
            query.Delete();
            Db.Connection.Close();
            return Ok();
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        [Route("add")]
        async public Task<IActionResult> Post([FromBody]DeviceInsertUpdate body)
        {
            Db.Connection.Open();
            var que = new DeviceInsertUpdate(Db);
            await que.addDevice(body);
            Db.Connection.Close();
            return Ok();
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        [Route("assign")]
        async public Task<IActionResult> AssignDevice([FromBody]Assign body)
        {
            Db.Connection.Open();
            var que = new Assign(Db);
            await que.assignDevice(body);
            Db.Connection.Close();
            return Ok();
        }

        [Authorize(Roles = "admin")]
        [HttpPut]
        [Route("update/{device_id}")]
        async public Task<IActionResult> Put(int device_id, [FromBody]DeviceInsertUpdate body)
        {
            Db.Connection.Open();
            var query = new DeviceInsertUpdate(Db);
            body.device_id = device_id;
            await query.updateDevice(body);
            Db.Connection.Close();
            return Ok();
        }

        [HttpGet("specification")]
        public async Task<IActionResult> GetAllSpecification()
        {
            int pageNumber = Convert.ToInt32((string)HttpContext.Request.Query["page"]);
            int pageSize = Convert.ToInt32((string)HttpContext.Request.Query["page-size"]);
            await Db.Connection.OpenAsync();
            var query = new Specification(Db);
            var pager = PagedList<Specification>.ToPagedList(query.getAllSpecifications(), pageNumber, pageSize);
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(pager.getMetaData()));
            return new OkObjectResult(pager);
        }

        [HttpGet]
        [Route("spec/{specification_id}")]
        public IActionResult GetSpec(int specification_id)
        {
            Db.Connection.Open();
            var query = new Specification(Db);
            var result = query.getspecbyid(specification_id);
            Db.Connection.Close();
            return Ok(result);
        }



        [Authorize(Roles = "admin")]
        [HttpPost]
        [Route("addspecification")]
        async public Task<IActionResult> Postspec([FromBody]Specification body)
        {
            Db.Connection.Open();
            var que = new Specification(Db);
            await que.addspecification(body);
            Db.Connection.Close();
            return Ok();
        }

        [Authorize(Roles = "admin")]
        [HttpPut]
        [Route("updatespecification/{specification_id}")]
        async public Task<IActionResult> Putspec(int specification_id, [FromBody]Specification body)
        {
            Db.Connection.Open();
            var query = new Specification(Db);
            body.specification_id = specification_id;
            await query.updatespecification(body);
            Db.Connection.Close();
            return Ok();
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        [Route("type")]
        async public Task<IActionResult> PostTYPE([FromBody]TypeBrandModel body)
        {
            Db.Connection.Open();
            var que = new TypeBrandModel(Db);
            await que.addType(body);
            Db.Connection.Close();
            return Ok();
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        [Route("brand")]
        async public Task<IActionResult> PostBrand([FromBody]TypeBrandModel body)
        {
            Db.Connection.Open();
            var que = new TypeBrandModel(Db);
            await que.addbrand(body);
            Db.Connection.Close();
            return Ok();
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        [Route("model")]
        async public Task<IActionResult> Postmodel([FromBody]TypeBrandModel body)
        {
            Db.Connection.Open();
            var que = new TypeBrandModel(Db);
            await que.addmodel(body);
            Db.Connection.Close();
            return Ok();
        }



        [HttpGet]
        [Route("previous_device/{id}")]
        public async Task<IActionResult> GetOne(int id)
        {
            string ToSearch = (string)HttpContext.Request.Query["search"] ?? "";
            string ToSort = (string)HttpContext.Request.Query["sortby"] ?? "";
            string Todirection = (string)HttpContext.Request.Query["direction"] ?? "asc";
            int pageNumber = Convert.ToInt32((string)HttpContext.Request.Query["page"]);
            int pageSize = Convert.ToInt32((string)HttpContext.Request.Query["page-size"]);
            await Db.Connection.OpenAsync();
            var query = new devices(Db);
            var pager = PagedList<devices>.ToPagedList(query.getPreviousDevice(id, ToSearch, ToSort, Todirection), pageNumber, pageSize);
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(pager.getMetaData()));
            await Db.Connection.CloseAsync();
            if (pager is null)
                return new NotFoundResult();
            return new OkObjectResult(pager);
        }
        [HttpGet("current_device/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            string ToSearch = (string)HttpContext.Request.Query["search"] ?? "";
            string ToSort = (string)HttpContext.Request.Query["sortby"] ?? "";
            string Todirection = (string)HttpContext.Request.Query["direction"] ?? "asc";
            int pageNumber = Convert.ToInt32((string)HttpContext.Request.Query["page"]);
            int pageSize = Convert.ToInt32((string)HttpContext.Request.Query["page-size"]);
            await Db.Connection.OpenAsync();
            var query = new devices(Db);
            var pager = PagedList<devices>.ToPagedList(query.getCurrentDevice(id, ToSearch, ToSort, Todirection), pageNumber, pageSize);
           Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(pager.getMetaData()));
            await Db.Connection.CloseAsync();
            if (pager is null)
                return new NotFoundResult();
            return new OkObjectResult(pager);
        }
        public AppDb Db { get; }
    }

}
