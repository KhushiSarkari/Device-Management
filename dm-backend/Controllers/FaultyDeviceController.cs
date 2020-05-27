using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using dm_backend.Models;
using dm_backend.Data;

namespace dm_backend.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class FaultyDeviceController : ControllerBase
    {
         public IDeviceRepository _repo;
         public AppDb Db { get; }

        public FaultyDeviceController(AppDb db,IDeviceRepository repo)
        {
            Db = db;
            _repo = repo;
        }

        // [Authorize(Roles = "admin")]
        [AllowAnonymous]
        [HttpGet]
        public IActionResult GetDeviceLisT(int userId,string search,string serialNumber,string status,string sortAttribute,string direction)
        {
            // int userId = -1;
            // string searchField = "";
            // string serialnumber = null;
            // string sortField = "";
             string sortDirection = "asc";
            // // int page = -1;
            // // int size = -1;
            // string status = null;
            int pageNumber = Convert.ToInt32((string)HttpContext.Request.Query["page"]);
            int pageSize = Convert.ToInt32((string)HttpContext.Request.Query["page-size"]);

            if (!string.IsNullOrEmpty(HttpContext.Request.Query["serial-number"]))
                serialNumber = (HttpContext.Request.Query["serial-number"]);

            if (!string.IsNullOrEmpty(HttpContext.Request.Query["id"]))
                userId = Convert.ToInt32(HttpContext.Request.Query["id"]);

            if (!string.IsNullOrEmpty(HttpContext.Request.Query["search"]))
                search = HttpContext.Request.Query["search"];

            if (!string.IsNullOrEmpty(HttpContext.Request.Query["sort"]))
                sortAttribute = HttpContext.Request.Query["sort"];

            if (!string.IsNullOrEmpty(HttpContext.Request.Query["direction"]))
                sortDirection = HttpContext.Request.Query["direction"];

            if (!string.IsNullOrEmpty(HttpContext.Request.Query["status"]))
                status = HttpContext.Request.Query["status"];

          

           
            //  var fault = new FaultyDevice(Db);
            //  var pager = PagedList<FaultyDeviceModel>.ToPagedList(fault.getFaultyDevice(userId, searchField, serialnumber, status, sortField, sortDirection), pageNumber, pageSize);
            //  Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(pager.getMetaData()));
            // object result;
            // try
            // {
            //  result = fault.getFaultyDevice(userId, searchField, serialnumber, status, sortField, sortDirection);
            // }
            // catch(Exception e)
            // {
               
            //    return NoContent();
            
            //  }
          var result = _repo.getFaultyDevice(userId,search,serialNumber,status,sortAttribute,sortDirection);
           return Ok(JsonConvert.SerializeObject(result, new JsonSerializerSettings() 
   { 
       NullValueHandling = NullValueHandling.Ignore 
   }));
           

        }



       // [Authorize(Roles = "admin")]
        [AllowAnonymous]
        [HttpPut]
        [Route("resolve")]
        public IActionResult PutResolveRequest([FromBody]FaultyDeviceModel fault)
        {
            string result = null;
             try
            {
                result = _repo.ResolveRequest(fault.complaintId);
            }
            catch (Exception n)
            {
              Console.WriteLine(n.Message);
            }
            return Ok(result);
        }

        //[Authorize(Roles = "admin")]
        [AllowAnonymous]
        [HttpPut]
        [Route("markfaulty")]
        public IActionResult PutReportFaultyRequest([FromBody]FaultyDeviceModel faulty)
        {       
            string result = null;
            try
            {
                result = _repo.MarkFaultyRequest(faulty.complaintId);
            }
            catch (Exception n)
            {
                Console.WriteLine(n.Message);
            }         
            return Ok(result);
        }
    }
}
