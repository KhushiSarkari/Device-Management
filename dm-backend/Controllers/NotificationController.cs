using System;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using dm_backend.Data;
using dm_backend.EFModels;
using Newtonsoft.Json;

namespace dm_backend.Controllers
{

    // [Authorize]
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase
    {
        public INotificationRepository _repo;
        public NotificationController(INotificationRepository repo)
        {
            _repo = repo;
        }
        [HttpPost]
        public IActionResult PostMultipleNotifications([FromBody]List<Notification> item)
        {
            try
            {
                _repo.AddMultipleNotifications(item);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        
        [HttpGet]
        public IActionResult GetNotification([FromQuery]BaseQueryParams Qparams)
        {
            var result = _repo.GetAllNotifications(Qparams);
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
            var result = _repo.GetNotificationCount(id);
            return Ok(result);
        }


    }

}