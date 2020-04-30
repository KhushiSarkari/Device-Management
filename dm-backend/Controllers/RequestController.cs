﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using dm_backend.Logics;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using dm_backend.Utilities;
using Newtonsoft.Json;

namespace dm_backend.Models{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class RequestController : Controller
    {
        public AppDb Db { get; }

        public RequestController(AppDb db)
        {
            Db = db;
        }


        [HttpPost]
        [Route("device")]
        public async Task<IActionResult> postdeviceRequest([FromBody] DeviceRequest req)
        {
            Db.Connection.Open();
            var request = new Request(Db);
            
            try
            {
              var result =   request.addDevice(req);

              await request.sendMailToAdmin(req , result);

            }
            catch(MySql.Data.MySqlClient.MySqlException e)
            {
               return BadRequest("request is incorrect");
            }
            Db.Connection.Close();
            return Ok("request Sent");

        }

        [HttpGet]
        [Route("pending")]
        public IActionResult GetRequest()
        {
            int userId=  -1;
            string searchField=(string) HttpContext.Request.Query["search"] ?? "";
            string sortField=(string) HttpContext.Request.Query["sortby"] ?? "request_device_id";
            string sortDirection=(string)HttpContext.Request.Query["direction"] ?? "asc";
            int pageNumber=Convert.ToInt32((string)HttpContext.Request.Query["page"]);
            int pageSize=Convert.ToInt32((string)HttpContext.Request.Query["page-size"]);
            if(!string.IsNullOrEmpty(HttpContext.Request.Query["id"]))
            userId=Convert.ToInt32((string)HttpContext.Request.Query["id"]);
            switch (sortField.ToLower())
            {
                 case "name":
                    sortField = "concat(first_name ,'', if (middle_name is null, '' , concat(middle_name , ' ')) ,last_name)";
                    break;
                case "specification":
                    sortField = "concat(RAM,'', storage ,'' ,screen_size ,'',connectivity)";
                    break;
            }
            Db.Connection.Open();
            var requestObject = new RequestModel(Db);
            var pager=PagedList<RequestModel>.ToPagedList(requestObject.GetAllPendingRequests(userId,sortField,sortDirection,searchField),pageNumber,pageSize);
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(pager.getMetaData()));
            Db.Connection.Close();
            return Ok(pager);
        }

        [Authorize(Roles="admin")]
        [HttpGet]
        [Route("{requestId}")]
        public async Task<IActionResult> RequestActionsAsync(int requestId, [System.Web.Http.FromUri]int id)
        {
            string name = (string)HttpContext.Request.Query["name"] ?? "";
            string email = (string)HttpContext.Request.Query["email"] ?? "";
            string action=(string)HttpContext.Request.Query["action"];
            Db.Connection.Open();
            RequestModel query = new RequestModel(Db);
            query.requestId = requestId;


           try{

                query.DeviceRequestAction(id,action);
               await new RequestStatus().sendStatusMail(requestId, action, name, email);
           }
            catch(Exception e){
                Console.WriteLine(e.Message);
              return BadRequest("An error occured while performing the action");
            }
            Db.Connection.Close();
            return new OkObjectResult("Action performed successfully");
        }


        [HttpDelete]
        [Route("{requestId}/cancel")]
        public IActionResult DeleteRequest(int requestId)
        {
            Db.Connection.Open();
            RequestModel query = new RequestModel(Db);
            string result = null;
            try{
                result =query.CancelRequest(requestId);
            }
            catch(Exception e){
                Console.WriteLine(e.Message);
                return NoContent();
            }
            Db.Connection.Close();
            return  Ok(result);
        }
        
    }
}
