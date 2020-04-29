using System;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using dm_backend.Logics;
using dm_backend.Models;
using Microsoft.AspNetCore.Authorization;
using MySql.Data.MySqlClient;
using dm_backend.Data;
using dm_backend.Utilities;
using Newtonsoft.Json;

namespace dm_backend.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase
    {
        public AppDb Db { get; }
        private readonly EFDbContext _context;

        public NotificationController(AppDb db,EFDbContext context)
        {
            Db = db;
            _context=context;
        }
        
        [HttpPost]
        public IActionResult PostMultipleNotifications([FromBody]MultipleNotifications item)
        {
            Db.Connection.Open();
            item.Db = Db;
            var result = item.AddMultipleNotifications();
            Db.Connection.Close();
            return new OkObjectResult(item);
        }


        [HttpGet]
         public IActionResult GetNotification()
        {
            int userId=  -1;
            string searchField=(string) HttpContext.Request.Query["search"] ?? "";
            string sortField=(string) HttpContext.Request.Query["sort"] ?? "notification_date";
            string sortDirection=(string)HttpContext.Request.Query["direction"] ?? "asc";
            if(!string.IsNullOrEmpty(HttpContext.Request.Query["id"]))
            userId=Convert.ToInt32((string)HttpContext.Request.Query["id"]);
            int pageNumber=Convert.ToInt32((string)HttpContext.Request.Query["page"]);
            int pageSize=Convert.ToInt32((string)HttpContext.Request.Query["page-size"]);
            sortDirection = (sortDirection.ToLower()) == "asc" ? "ASC" : "DESC";
            switch (sortField.ToLower())
            {
                 case "device_name":
                    sortField = "concat(type ,'', brand , '' ,  model)";
                    break;
                case "specification":
                    sortField = "concat(RAM,'', storage ,'' ,screen_size ,'',connectivity)";
                    break;
                default:  sortField = "concat(type ,'', brand , '' ,  model)";

                break;
              
            }
            Db.Connection.Open();
            var NotificationObject = new NotificationModel(Db);
            var pager=PagedList<NotificationModel>.ToPagedList(NotificationObject.GetNotifications(userId,sortField,sortDirection,searchField),pageNumber,pageSize);
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(pager.getMetaData()));
            Db.Connection.Close();
            return Ok(pager);
        }

        
        [HttpGet]
         [Route("reject/{notificationId}")]
        public IActionResult RejectNotification(int notificationId)
        {
            Db.Connection.Open();
            using var cmd = Db.Connection.CreateCommand();
            
            cmd.CommandText = "reject_user_request";
            cmd.CommandType = CommandType.StoredProcedure; 
            try{
                cmd.Parameters.AddWithValue("@var_notif_id", notificationId);
                cmd.ExecuteNonQuery();
            }
            catch(Exception e){
                return NoContent();
            }
            Db.Connection.Close();
            
            return  Ok("Request rejected");
        }
        [AllowAnonymous]
        [HttpGet]
        [Route("Count/{id}")]
        public int GetCount(int id)
         {
            var values = _context.Notification.Count(w => (w.UserId == id)&& (w.StatusId==9) );
            return values;

        }


    }

}