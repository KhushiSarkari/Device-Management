using System;
using dm_backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using dm_backend.Models;
using dm_backend.Data;

using Newtonsoft.Json;
using dm_backend.Logics;
using System.Threading.Tasks;
using dm_backend.Utilities;
using System.Collections.Generic;
using AutoMapper;
using dm_backend.EFModels;

namespace dm_backend.Controllers
{
  //  [Authorize]
    [Route("api/[controller]")]
    public class UserController : BaseController
    {
         private IUserRepository _userrepo ;
         private readonly IMapper _mapper ; 
        public UserController(AppDb db, EFDbContext ef , IUserRepository repo ,IMapper mapper): base(ef)
        {
    
            _userrepo= repo ;
            _mapper =mapper;

        }
        // [Authorize(Roles="admin,user")]
        // [HttpGet]
        // [Route("{user_id}")]
        // public  JsonResult GetOneUser(string user_id)
        // {
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
        //     Db.Connection.Open();
        //     var query = new User(Db);
        //     var result = query.getUserByuser_id(user_id);
        //     result.SetSerializableProperties(String.Empty);
        //     Db.Connection.Close();
        //     return Json(result);
        // }
       
       
      //  [Authorize(Roles="admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllUsersInCustomFormat([FromQuery]UserParams userParams)
        {
             var users = await _userrepo.GetUsers(userParams);
          //   var userToReturn = _mapper.Map<IEnumerable<UserForListDto>>(users);
             Response.AddPagination(users.CurrentPage , users.PageSize , users.TotalCount , users.TotalPages);

             return Ok(users);
            

        }
        
        // [Authorize(Roles="admin")]
        // [HttpPost]
        // [Route("add")]
        // public IActionResult Post([FromBody]User item)
        // {
        //     Db.Connection.Open();
        //     item.Db = Db;
        //     var result = item.AddOneUser();
        //     Db.Connection.Close();
        // string body ="Congratulations !<br>"+item.FirstName+" "+item.LastName+"<br>  Your account has been created on Device Management portal  <br> Thanks  ";
        // var sendobj = new sendMail().sendNotification(item.Email,body,"Registration Successfull") ;
        //     return new OkObjectResult(item);
        // }

        // [Authorize(Roles="admin,user")]
        // [HttpPut]
        // [Route("{user_id}/update")]
        // public ActionResult Put(int user_id, [FromBody]User body)
        // {
        //     Db.Connection.Open();
        //     body.Db = Db;
        //     body.UserId = user_id;
        //     Console.WriteLine(body.FirstName);
        //     var result=  body.UpdateUser();
        //     Db.Connection.Close();
        //     return Ok(result);
    
        // }

        // [Authorize(Roles="admin")]
        // [HttpGet]
        // [Route("{user_id}/{activeInactive}")]
        // public IActionResult PutOne(int user_id , string activeInactive)
        // {
        //     Db.Connection.Open();
        //     var query = new User(Db);
        //     query.UserId = user_id;
        //     query.MarkUserInactive(query.whatIs(activeInactive));
        //     Db.Connection.Close();
        //     return Ok();
        // }
        // [Authorize(Roles="admin")]
        // [HttpDelete]
        // [Route("{user_id}/remove")]
        // public IActionResult DeleteOne(int user_id)
        // {
        //     Db.Connection.Open();
        //     User query = new User(Db)
        //     {
        //         UserId = user_id
        //     };
        //     query.Delete();
        //     Db.Connection.Close();
        //     return  Ok();
        // }
       
        // public AppDb Db { get; }
    }

}
