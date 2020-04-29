using System;
using dm_backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using dm_backend.Models;
using Newtonsoft.Json;
using dm_backend.Logics;

namespace dm_backend.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class UserController : BaseController
    {

        public UserController(AppDb db)
        {
            Db = db;
        }
        [Authorize(Roles="admin,user")]
        [HttpGet]
        [Route("{user_id}")]
        public  JsonResult GetOneUser(string user_id)
        {
            /*
                **** Get the user Id as (int)
                Console.WriteLine("User id: " + GetUserId());

                **** Get the user email as (string)
                Console.WriteLine("User name: " + GetUserName());

                **** Get the user roles as (List of strings)
                foreach(string roleName in GetUserRoles()){
                    Console.WriteLine("Role: ", roleName);
                }
            */
            Db.Connection.Open();
            var query = new User(Db);
            var result = query.getUserByuser_id(user_id);
            result.SetSerializableProperties(String.Empty);
            Db.Connection.Close();
            return Json(result);
        }
       
       
        [Authorize(Roles="admin")]
        [HttpGet]
        public JsonResult GetAllUsersInCustomFormat()
        {
            string fieldsToDisplay = HttpContext.Request.Query["fields"];
            string namesToSearch = HttpContext.Request.Query["search"];
            string ToSort = (string)HttpContext.Request.Query["sortby"] ?? "first_name";
            string direction = (string)HttpContext.Request.Query["direction"]  ?? "ASC" ;
            int pageNumber=Convert.ToInt32((string)HttpContext.Request.Query["page"]);
            int pageSize=Convert.ToInt32((string)HttpContext.Request.Query["page-size"]);
            Db.Connection.Open();
            var query = new User(Db);
             var pager=PagedList<User>.ToPagedList(query.SortUserbyName(ToSort, direction, namesToSearch),pageNumber,pageSize);
            Response.Headers.Add("X-Pagination",JsonConvert.SerializeObject(pager.getMetaData()));
            Db.Connection.Close();
            foreach (var m1 in pager)
            {
                m1.SetSerializableProperties(fieldsToDisplay);
            }
            return Json(pager);
            

        }
        
        [Authorize(Roles="admin")]
        [HttpPost]
        [Route("add")]
        public IActionResult Post([FromBody]User item)
        {
            Db.Connection.Open();
            item.Db = Db;
            var result = item.AddOneUser();
            Db.Connection.Close();
        string body ="Congratulations !<br>"+item.FirstName+" "+item.LastName+"<br>  Your account has been created on Device Management portal  <br> Thanks  ";
        var sendobj = new sendMail().sendNotification(item.Email,body,"Registration Successfull") ;
            return new OkObjectResult(item);
        }

        [Authorize(Roles="admin,user")]
        [HttpPut]
        [Route("{user_id}/update")]
        public ActionResult Put(int user_id, [FromBody]User body)
        {
            Db.Connection.Open();
            body.Db = Db;
            body.UserId = user_id;
            Console.WriteLine(body.FirstName);
            var result=  body.UpdateUser();
            Db.Connection.Close();
            return Ok(result);
    
        }

        [Authorize(Roles="admin")]
        [HttpGet]
        [Route("{user_id}/{activeInactive}")]
        public IActionResult PutOne(int user_id , string activeInactive)
        {
            Db.Connection.Open();
            var query = new User(Db);
            query.UserId = user_id;
            query.MarkUserInactive(query.whatIs(activeInactive));
            Db.Connection.Close();
            return Ok();
        }
        [Authorize(Roles="admin")]
        [HttpDelete]
        [Route("{user_id}/remove")]
        public IActionResult DeleteOne(int user_id)
        {
            Db.Connection.Open();
            User query = new User(Db)
            {
                UserId = user_id
            };
            query.Delete();
            Db.Connection.Close();
            return  Ok();
        }
       
        public AppDb Db { get; }
    }

}
