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

namespace dm_backend.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class ReturnRequestController : ControllerBase
    {
        public AppDb Db { get; }

        public ReturnRequestController(AppDb db)
        {
            Db = db;
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
            }
            catch(NullReferenceException){
                return NoContent();
            }
            Db.Connection.Close();
            return Ok(result);
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
                return NoContent();
            }
            Db.Connection.Close();
            
            return  Ok("Action successfully performed");
        }
    }

}