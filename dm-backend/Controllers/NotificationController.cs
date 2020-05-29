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
using Newtonsoft.Json;

namespace dm_backend.Controllers
{

    // [Authorize]
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase
    {
        public AppDb Db { get; }
        public INotificationRepository _repo;
        public ISendMail _send;
        public NotificationController( INotificationRepository repo,ISendMail mail)
        {
            _repo = repo;
            _send = mail;
        }
        // private readonly EFRepository _context;

        // public NotificationController(AppDb db,EFDbContext context)
        // {
        //     Db = db;
        //     _context=context;
        // }
        
        [HttpPost]
        public IActionResult PostMultipleNotifications([FromBody]MultipleNotifications item)
        {
            try
            {
                Task.WhenAll(_repo.sendMultipleMail(item));
            }
            catch
            {
                return BadRequest();
            }
            var result = _repo.AddMultipleNotifications(item);
           
            return new OkObjectResult(item);
        }

        
        [HttpGet]
        public IActionResult GetNotification()
        {
             int userId = -1;
            string searchField = (string)HttpContext.Request.Query["search"] ?? "";
             string sortField = (string)HttpContext.Request.Query["sort"] ?? "notification_date";
             string sortDirection = (string)HttpContext.Request.Query["direction"] ?? "asc";
            if (!string.IsNullOrEmpty(HttpContext.Request.Query["id"]))
                userId = Convert.ToInt32((string)HttpContext.Request.Query["id"]);
            int pageNumber = Convert.ToInt32((string)HttpContext.Request.Query["page"]);
            int pageSize = Convert.ToInt32((string)HttpContext.Request.Query["page-size"]);
            sortDirection = (sortDirection.ToLower()) == "asc" ? "ASC" : "DESC";
            
            // Db.Connection.Open();
            // var NotificationObject = new NotificationModel(Db);
            // var pager = PagedList<NotificationModel>.ToPagedList(NotificationObject.GetNotifications(userId, sortField, sortDirection, searchField), pageNumber, pageSize);
            // Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(pager.getMetaData()));
            // Db.Connection.Close();
            var result = _repo.GetAllNotifications(userId, sortField, sortDirection, searchField);
            return Ok(result);
        }


        [AllowAnonymous]
        [HttpPut]
         [Route("reject/{notificationId}")]
        public IActionResult RejectNotification(int notificationId)
        {
            var result = _repo.RejectNotification(notificationId);
            return Ok(result);
        }
        [AllowAnonymous]
        [HttpGet]
        [Route("Count/{id}")]
        public IActionResult GetCount(int id)
         {
            var result = _repo.GetNotification(id);
            return Ok(result);
        }


    }

}