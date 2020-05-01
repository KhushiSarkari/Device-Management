using System;
using System.Threading.Tasks;
using dm_backend.Data;
using dm_backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
namespace dm_backend.Controllers
{
    [Authorize(Roles="admin")]
    [Route("api/")]
    public class RolepermissionController : BaseController
    {
        public RolepermissionController(AppDb db, EFDbContext ef): base(ef)
        {
            Db = db;
        }
        public AppDb Db { get; }
        
        /*
        *   Returns an JSON object with an array of Roles along with an array of permissions
        *   {
        *     Roles: [
        *       { 
        *         RoleName, 
        *         Permissions: [
        *           {PermissionName}
        *         ]
        *       }
        *     ]
        *   }
        */
        [HttpGet]
        [Route("rolepermission")]
        public IActionResult getAllRoles(){
            var result = new RolePermission(Db).GetAllRoles();
            string JSON = JsonConvert.SerializeObject(result, new JsonSerializerSettings(){
                NullValueHandling = NullValueHandling.Ignore
            });
            return Ok(JSON);
        }

        [HttpGet]
        [Route("role/{role_id}")]
        public IActionResult getRoleById(int role_id){
            Role RoleObj = new Role(Db);
            return Ok(RoleObj.GetRoleById(role_id));
        }
        
        /*
        *   Requires the same object as received from GET endpoint with updated values
        */
        [HttpPut]
        [Route("rolepermission/update")]
        public IActionResult UpdateRoles([FromBody]RolePermission RolePerms){
            RolePerms.Db = Db;
            try{
                RolePerms.SaveChanges();
            }
            catch(Exception e){
                return StatusCode(500);
            }
            return Ok();
        }

        [HttpPost]
        [Route("role/add")]
        public IActionResult Postrole([FromBody]Role body)
        {
            try{
                body.Db = Db;
                body.AddRole();
                return Ok();
            }
            catch(Exception e){
                Console.WriteLine(e.Message);
                return BadRequest();
            }
        }
        [HttpPost]
        [Route("permission/add")]
        public IActionResult Postpermission([FromBody]Permission body)
        {
            try{
                body.Db = Db;
                body.AddPermission();
                return Ok();
            }
            catch(Exception e){
                Console.WriteLine(e.Message);
                return BadRequest();
            }
        }
        [HttpPut]
        [Route("role/{role_id}/update")]
        public IActionResult Putrole(int role_id, [FromBody]Role body)
        {
            try{
                body.Db = Db;
                body.RoleId = role_id;
                body.UpdateRole();
                return Ok();
            }
            catch(Exception e){
                Console.WriteLine(e.Message);
                return BadRequest();
            }
            
        }
        [HttpPut]
        [Route("permission/{permission_id}/update")]
        public IActionResult Putpermission(int permission_id, [FromBody]Permission body)
        {
            try{
                body.Db = Db;
                body.PermissionId = permission_id;
                body.UpdatePermission();
                return Ok();
            }
            catch(Exception e){
                Console.WriteLine(e.Message);
                return BadRequest();
            }
        }
        [HttpDelete]
        [Route("role/{role_id}/delete")]
        public IActionResult Deleterole(int role_id)
        {
            Role query = new Role(Db);
            query.RoleId = role_id;
            if(query.Deleterole()==1)
            {
                 return Ok();
            }
            else{
                return BadRequest();
            }
            
        }
        [HttpDelete]
        [Route("permission/{permission_id}/delete")]
        public IActionResult Deletepermission(int permission_id)
        {
            Permission query = new Permission(Db);
            query.PermissionId = permission_id;
            if(query.DeletePermission()==1)
            {
                 return Ok();
            }
            else{
                return BadRequest();
            }
            
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("is_user")]
        public IActionResult AmIUser(){
            return Ok(new{ result= GetUserRoles().Contains("user") });
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("is_admin")]
        public IActionResult AmIAdmin(){
            return Ok(new{ result= GetUserRoles().Contains("admin") });
        }
        [AllowAnonymous]
        [HttpGet]
        [Route("canI/{permission_name}")]
        public IActionResult DoIHavePermission(string permission_name){
            return Ok(new{ result = GetUserPermissions().Contains(permission_name) });
        }
    }
}