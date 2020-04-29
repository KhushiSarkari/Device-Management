using System;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using dm_backend.Models;
using Microsoft.AspNetCore.Authorization;
using MySql.Data.MySqlClient;
using dm_backend.Utilities;
using Newtonsoft.Json;
using dm_backend.Data;
using dm_backend.Logics;

namespace dm_backend.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class ReturnRequestController : ControllerBase
    {
        public AppDb Db { get; }
        private readonly EFDbContext _context;

        public ReturnRequestController(AppDb db, EFDbContext context)
        {
            Db = db;
            _context = context ;
        }

        [HttpPost]
        public IActionResult PostReturnRequest([FromBody]ReturnRequestModel request)
        {
            Db.Connection.Open();
            request.Db = Db;
            string result = null;
            try{
                result = request.AddReturnRequest();
            }
            catch(NullReferenceException){
                return NoContent();
            }
            Db.Connection.Close();
            return Ok(result);
        }
        
         [HttpPost]
         [Route("fault")]
        public IActionResult PostFaultRequest([FromBody]ReturnRequestModel request)
        {
            Db.Connection.Open();
            request.Db = Db;
            
            
            string result = null;
            try{
                result = request.AddFaultRequest();
                var obj = ToUser(request.deviceId);
            string body = "Hey Admin ! <br> Device "+obj[2]+" "+obj[3]+" having Serial Number "+obj[1]+" has Following Complaints <br><br>"+request.comment+"<br>Thanks";
              var EmailObj = new sendMail().sendNotification(obj[0],body,"Device Complaint");
              }
            catch(NullReferenceException){
                return NoContent();
            }

            


            Db.Connection.Close();


            return Ok(result);
        }

        private string[] ToUser(int devid)
        {
             var entryPoint = (from us in _context.User
                              join ad in _context.AssignDevice on us.UserId equals ad.ReturnTo
                              join  dv in _context.Device on ad.DeviceId equals dv.DeviceId
                              join dt in _context.DeviceType on dv.DeviceTypeId equals dt.DeviceTypeId
                              join br in _context.DeviceBrand on dv.DeviceBrandId equals br.DeviceBrandId
                              join md in _context.DeviceModel on dv.DeviceModelId equals md.DeviceModelId
                              where ad.DeviceId == devid
                              select new
                              { Model = md.Model,
                                DeviceType = dt.Type,
                               SerialNumber =dv.SerialNumber,  
                                email = us.Email 
                              }).ToList();
                  var x= entryPoint[0];
                  string[] str1; 
                  str1 = new String[]{x.email, x.SerialNumber,x.DeviceType,x.Model };
                  
                  return str1;
                
        }

        [HttpGet]
        public IActionResult GetReturnRequest()
        {
            int userId=  -1;
            string searchField=(string) HttpContext.Request.Query["search"] ?? "";
            string sortField=(string) HttpContext.Request.Query["sort"] ?? "";
            string sortDirection=(string)HttpContext.Request.Query["direction"] ?? "asc";
            if(!string.IsNullOrEmpty(HttpContext.Request.Query["id"]))
            userId=Convert.ToInt32((string)HttpContext.Request.Query["id"]);
            int pageNumber=Convert.ToInt32((string)HttpContext.Request.Query["page"]);
            int pageSize=Convert.ToInt32((string)HttpContext.Request.Query["page-size"]);

            Db.Connection.Open();
            var returnObject = new ReturnRequestModel(Db);
            var pager=PagedList<ReturnRequestModel>.ToPagedList(returnObject.GetReturnRequests(userId,sortField,sortDirection,searchField),pageNumber,pageSize);
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(pager.getMetaData()));
            Db.Connection.Close();
            return new OkObjectResult(pager);
        }

        [Authorize(Roles="admin")]
        [HttpGet]
        [Route("{returnId}")]
        public IActionResult ReturnActions(int returnId, [System.Web.Http.FromUri]int id)
        {
            string action=(string)HttpContext.Request.Query["action"];
            Db.Connection.Open();
            using var cmd = Db.Connection.CreateCommand();
            if(action=="accept")
                cmd.CommandText = "accept_return";
            else if(action=="reject")
                cmd.CommandText = "reject_return";
            cmd.CommandType = CommandType.StoredProcedure; 
            try{
                cmd.Parameters.AddWithValue("@return_id", returnId);
                cmd.Parameters.Add(new MySqlParameter("var_admin_id", id));
                cmd.ExecuteNonQuery();
            }
            catch(Exception e){
                Console.WriteLine(e);
                return NoContent();
            }
            Db.Connection.Close();
            
            return  Ok("Action successfully performed");
        }
    }

}