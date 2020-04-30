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
        [Route("role/update")]
        public IActionResult Postrole([FromBody]Role body)
        {
            try{
                body.Db = Db;
                if(body.RoleId.HasValue){
                    body.UpdateRole();
                }
                else{
                    body.AddRole();
                }
                return Ok();
            }
            catch(Exception e){
                Console.WriteLine(e.Message);
                return BadRequest();
            }
        }
        [HttpPost]
        [Route("permission/update")]
        public IActionResult Postpermission([FromBody]Permission body)
        {
            try{
                body.Db = Db;
                if(body.PermissionId.HasValue){
                    body.UpdatePermission();
                }
                else{
                    body.AddPermission();
                }
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
            query.Deleterole();
            return Ok();
        }
        [HttpDelete]
        [Route("permission/{permission_id}/delete")]
        public IActionResult Deletepermission(int permission_id)
        {
            Permission query = new Permission(Db);
            query.PermissionId = permission_id;
            query.DeletePermission();
            return Ok();
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