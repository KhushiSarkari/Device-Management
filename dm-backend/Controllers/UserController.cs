using System;
using dm_backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using dm_backend.Models;
using dm_backend.Data;
using System.Linq;
using Newtonsoft.Json;
using dm_backend.Logics;
using System.Collections.Generic;
using dm_backend.EFModels;

namespace dm_backend.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class UserController : BaseController
    {
        private readonly EFDbContext _context;
        public IAuthRepository _Irepo;
        public IUserRepository _repo;
        public UserController(AppDb db , IUserRepository repo, EFDbContext context,IAuthRepository Irepo): base(context)
                {
                    Db = db;
                    _repo = repo;
                    _context = context;
                    _Irepo = Irepo;
                }
        
        
        [Authorize(Roles="admin,user")]
        [HttpGet]
        [Route("{user_id}")]
        public IActionResult GetOneUser(int user_id)
        {
            var result  = _repo.GetOneUser(user_id);
          
             return Ok(result);
        }
       
       
        [Authorize(Roles="admin")]
        [HttpGet]
        public IActionResult GetAllUsersInCustomFormat()
        {
            string fieldsToDisplay = HttpContext.Request.Query["fields"];
            string namesToSearch = HttpContext.Request.Query["search"];
            string ToSort = (string)HttpContext.Request.Query["sortby"] ?? "first_name";
            string direction = (string)HttpContext.Request.Query["direction"]  ?? "ASC" ;
            int pageNumber=Convert.ToInt32((string)HttpContext.Request.Query["page"]);
            int pageSize=Convert.ToInt32((string)HttpContext.Request.Query["page-size"]);
            //Console.WriteLine(fieldsToDisplay+"\n" +namesToSearch+"\n" +ToSort+"\n" +direction+"\n" +pageNumber+"\n" +pageSize);
       
            var Result = _repo.GetAllUsers(namesToSearch);
            Response.Headers.Add("X-Pagination","{\"TotalCount\":20,\"PageSize\":4,\"CurrentPage\":1,\"TotalPages\":5,\"HasNext\":true,\"HasPrevious\":false}");
                        if (Result.Count() > 0)
                            return  Ok(Result);
                        else
                            return NoContent();

          //  Db.Connection.Open();
          //  var query = new User(Db);
           //  var pager=PagedList<User>.ToPagedList(SortUserbyName(ToSort, direction, namesToSearch),pageNumber,pageSize);
            // Response.Headers.Add("X-Pagination",JsonConvert.SerializeObject(pager.getMetaData()));
         //   Db.Connection.Close();
            // foreach (var m1 in pager)
            // {
            //     m1.SetSerializableProperties(fieldsToDisplay);
            // }
            // return Json(pager);
            

        }

       
        
        [Authorize(Roles="admin")]
        [HttpPost]
        [Route("add")]
        public IActionResult Post([FromBody]Models.User item)
        {
           
            _repo.PostUser(item);
            return Ok();
 // _context.User.AddAsync(item);
            //  return Ok();
          //  Db.Connection.Open();
        //     item.Db = Db;
        //     try
        //     {
        //     var result = item.AddOneUser();
        //     Db.Connection.Close();
        // string body ="Congratulations !<br>"+item.FirstName+" "+item.LastName+"<br>  Your account has been created on Device Management portal  <br> Thanks  ";
        // var sendobj = new sendMail().sendNotification(item.Email,body,"Registration Successfull") ;
        //     }
        //     catch
        //     {
        //         return BadRequest("Registration Failed. Email must be Unique");
        //     }
        //     return new OkObjectResult(item);
        }



        
        [Authorize(Roles="admin,user")]
        [HttpPut]
        [Route("{user_id}/update")]
        public ActionResult Put(int user_id, [FromBody]Models.User body)
        {
          _repo.UpdateUser(user_id,body);
          return Ok();
        }
         
        [Authorize(Roles="admin")]
        [HttpGet]
        [Route("{user_id}/{activeInactive}")]
        public IActionResult PutOne(int user_id , string activeInactive)
        {
          _context.User.FirstOrDefault(e=>e.UserId==user_id).Status=(activeInactive=="inactive")?2:1;
          _context.SaveChanges();
          return Ok();
       }
        
        
        [Authorize(Roles="admin")]
        [HttpDelete]
        [Route("{user_id}/remove")]
        public IActionResult DeleteOne(int user_id)
        {
           _repo.DeleteUser(user_id);
           return Ok();
         }
       
        public AppDb Db { get; }
    }

}
