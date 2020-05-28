using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.EntityFrameworkCore.Internal;
using dm_backend.Utilities;
using dm_backend.Logics;

using System;
using System.Linq;
using System.Threading.Tasks;
using dm_backend.Data;
using dm_backend.EFModels;
using dm_backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
namespace dm_backend.Controllers
{    
    [Authorize(Roles="admin")]
     [ApiController]
     [Route("api/")]
    public class RolepermissionController : BaseController
    {
                 private readonly EFDbContext _context ;
         public IRoleRepository _repo;
        public RolepermissionController( EFDbContext ef,IRoleRepository repo): base(ef)
        {
         //   Db = db;

            _context = ef ;
             _repo = repo;

        }

     [HttpPost("role/update")]
        public async Task<IActionResult> Postrole(Models.Role obj)
        {
             var _resonce =await _repo.Postrole(obj);
            
              if(_resonce!=null){

            return Ok(new { Result = _resonce});
            }
            else{
                return BadRequest();
            }

        }
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
        public  async Task<IActionResult> getAllRoles(){

             var _resonce =await _repo.getAllRoles();
            
             return Ok(_resonce);

        }
           [HttpPost("permission/update")]
        public async Task<IActionResult> PostPerm(Models.Permission data1)
        {
            var _resonce =await _repo.PostPerm(data1);
            
              if(_resonce!=null){

            return Ok(_resonce);
            }
            else{
                return BadRequest();
            }

        }

    [HttpPut]
        [Route("rolepermission/update")]
        public  async Task<IActionResult> UpdateRoles(Models.RolePermission RolePerms)
        {
             var _resonce =await _repo.UpdateRoles(RolePerms);
              return Ok(new { Result = _resonce});
        }

        [HttpDelete]
        [Route("role/{role_id}/delete")]
        public async Task<IActionResult> Deleterole(int role_id)
        {
            var _resonce=await _repo.Deleterole(role_id);
            if(_resonce!=null){

            return Ok(_resonce);
            }
            else{
                return BadRequest();
            }
        }

          [HttpDelete]
        [Route("permission/{permission_id}/delete")]
        public async Task<IActionResult> DeletePerm(int permission_id)
        {
            var _resonce=await _repo.DeletePerm(permission_id);
             if(_resonce!=null){

            return Ok(_resonce);
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
