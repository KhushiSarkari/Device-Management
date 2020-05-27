// using System;
// using System.Threading.Tasks;
// using dm_backend.Data;
// using dm_backend.EFModels;
// using dm_backend.Models;
// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Mvc;
// using Newtonsoft.Json;
// using System.Linq;
// using Microsoft.EntityFrameworkCore;
// using System.Collections.Generic;
// using Microsoft.AspNetCore.Http;
// using Microsoft.AspNetCore.Mvc;
// using System.Threading.Tasks;
// using System;
// using System.Collections.Generic;
// using System.Security.Claims;
// using System.Text;
// using Microsoft.Extensions.Configuration;
// using dm_backend.Data;
// using dm_backend.EFModels;
// using Microsoft.IdentityModel.Tokens;
// using System.IdentityModel.Tokens.Jwt;
// using System.Linq;
// using Microsoft.EntityFrameworkCore.Internal;
// using dm_backend.Utilities;
// using Newtonsoft.Json;
// using dm_backend.Logics;


// namespace dm_backend.Controllers
//  {
//     [Authorize(Roles="admin")]
    // [Route("api/")]
    // public class RolepermissionController : BaseController
    // {
    //     public readonly EFDbContext _context;
    //     public RolepermissionController(AppDb db, EFDbContext ef): base(ef)
    //     {
    //         Db = db;
    //         _context=ef;
    //     }
    //     public AppDb Db { get; }
        
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
    // [Authorize(Roles="admin")]
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
        // public AppDb Db { get; }

        [HttpGet]
        [Route("rolepermission")]
        public IActionResult getAllRoles(){
      
                                    var perms = (from p in _context.Permission select new Models.Permission{
                            PermissionId=p.PermissionId,
                            PermissionName=p.PermissionName
                                });

                            var role1= (from r in _context.Role select new Models.Role{
                                RoleId= r.RoleId, 
                                RoleName=r.RoleName,
                                Permissions= (from rp in _context.RoleToPermission join p in perms on rp.PermissionId equals
                                        p.PermissionId where rp.RoleId==r.RoleId 
                                            select new Models.Permission{
                                                PermissionId= p.PermissionId,
                                                PermissionName=p.PermissionName
                                            }).ToList()
                                        });
        var rp = new Models.RolePermission{
                Roles=role1.ToList(),
                Permissions=perms.ToList()
                };

            return Ok(rp);
        }

        // [HttpGet]
        // [Route("role/{role_id}")]
        // public IActionResult getRoleById(int role_id){
        //     Role RoleObj = new Role(Db);
        //     return Ok(RoleObj.GetRoleById(role_id));
        // }
        
        /*
        *   Requires the same object as received from GET endpoint with updated values
        */
        // [HttpPut]
        // [Route("rolepermission/update")]
        // public IActionResult UpdateRoles([FromBody]Models.RolePermission RolePerms){
        //     RolePerms.Db = Db;
        //     try{
        //         RolePerms.SaveChanges();
        //     }
        //     catch(Exception e){
        //         return StatusCode(500);
        //     }
        //     return Ok();
        // }

         [HttpPut]
        [Route("rolepermission/update")]
        public  async Task<IActionResult> UpdateRoles(Models.RolePermission RolePerms)
        {
            //     var transaction =  _context.Database.BeginTransaction();
            // Console.WriteLine(RolePerms.Roles[0].RoleId);
            //  try
            // {

                   
       
                
               foreach (Models.Role roleobj in RolePerms.Roles)
               {

                     var x=(_context.Role.FirstOrDefault(e=>e.RoleName==roleobj.RoleName).RoleId);
                    // var y=(Models.Permission.FirstOrDefault(e=>e.PermissionName==Models.Permission.Permissions.RoleName));
                    //  var y=(Models.Permission.Permission.FirstOrDefault(e=>e.PermissionName==Models.Permission.Permissions));
                    // Console.WriteLine(x);
                    // var x=roleobj.RoleName;(_context.Role.Where(e=>e.RoleName==roleobj.RoleName).RoleId)
                //    var role_del=_context.RoleToPermission.FirstOrDefault(e=>e.RoleId=roleobj.RoleId);
                
                
                     _context.RoleToPermission.RemoveRange(_context.RoleToPermission.Where(e=> e.RoleId==x));
                    
                    // var objdel=_context.RoleToPermission.FirstOrDefault(e=>e.RoleId==del1.RoleId);
                    //  _context.RoleToPermission.Remove();
                    // Console.WriteLine(roleobj.RoleName);
                 _context.SaveChanges();

                //  }
                // foreach(Models.Role rl in RolePerms.Roles){
              
                 if(roleobj.Permissions!=null){
                     
                    foreach(Models.Permission permobj in roleobj.Permissions)
                    {
                        //  Console.WriteLine(permobj.PermissionId+" "+rl.RoleId);
                         var perm_id=_context.Permission.FirstOrDefault(e=>e.PermissionName==permobj.PermissionName).PermissionId;
                         var role_id1=_context.Role.FirstOrDefault(e=>e.RoleName==roleobj.RoleName).RoleId;
                       var perm_int=new EFModels.RoleToPermission();
                       perm_int.RoleId= role_id1;
                       perm_int.PermissionId=perm_id;
                        Console.WriteLine(perm_int.PermissionId+" "+perm_int.RoleId);
                           _context.RoleToPermission.Add(perm_int);
                           
              
                               _context.SaveChanges();
                               


                    }
                 }
                //  else{
                //        foreach(Models.Permission permobj1 in roleobj.Permissions)
                //     {
                //         //  Console.WriteLine(permobj.PermissionId+" "+rl.RoleId);
                //          var perm_id1=_context.Permission.FirstOrDefault(e=>e.PermissionName==permobj1.PermissionName).PermissionId;
                //          var role_id2=_context.Role.FirstOrDefault(e=>e.RoleName==roleobj.RoleName).RoleId;
                //        var perm_int1=new EFModels.RoleToPermission();
                //        perm_int1.RoleId= role_id2;
                //        perm_int1.PermissionId=perm_id1;
                //         Console.WriteLine(perm_int1.PermissionId+" del"+perm_int1.RoleId);
                //            _context.RoleToPermission.Add(perm_int1);
                           
              
                //                _context.SaveChanges();
                               


                //     }
                //  }
                    
                    
               }
              
               
                
        // }
                // return Ok();
            
                // if(roleobj.Permissions!=null){
                //     foreach(Models.Permission permobj in roleobj.Permissions)
                //     {
                //          Console.WriteLine(permobj.PermissionId+" "+roleobj.RoleId);
                //        var perm_int=new EFModels.RoleToPermission();
                //        perm_int.RoleId= (int)roleobj.RoleId;
                //        perm_int.PermissionId=(int)permobj.PermissionId;
                       
                //            _context.RoleToPermission.Add(perm_int);
              
                //              _context.SaveChanges();

                //     }

    
    // }
            // }
                
            
            // catch (Exception ex)
            // {
            //     transaction.Rollback();
            //     Console.WriteLine(ex);
               
            // }
    
  return Ok();
               }
          
      

           [HttpPost("role/update")]
        public async Task<IActionResult> Postrole(Models.Role obj)
        {
            Console.WriteLine(obj.RoleId+"iiii");
            if(obj.RoleId.HasValue)
            {            
            // var Roleobj=new EFModels.Role { RoleId= obj.RoleId,RoleName=obj.RoleName};
                if(await _context.Role.AnyAsync(e=>e.RoleId==obj.RoleId) )
                {
                    var  user =  _context.Role.FirstOrDefault(e=>e.RoleId==obj.RoleId.Value);
                    user.RoleName=obj.RoleName;
                    // await _context.SaveChangesAsync();
                }
                else{
                    return BadRequest();
                }
            }
              else
               {
                   var Roleobj=new EFModels.Role { RoleName=obj.RoleName};
                  await _context.Role.AddAsync(Roleobj);
                //  return Ok(Roleobj);
                }
             await _context.SaveChangesAsync();

             return Ok();
}

           [HttpPost("permission/update")]
        public async Task<IActionResult> PostPerm(Models.Permission data1)
        {
            var _resonce =await _repo.PostPerm(data1);
             return Ok(new { Result = _resonce});

        }

      
        // [HttpPost]
        // [Route("role/update")]

      

        // public  async Task<IActionResult> Postrole(DtoRole obj)
        // {
        //     Console.WriteLine(obj.RoleName+"jjjj");
                // body.Db = Db;
                // if(body.RoleId.HasValue){
                //     body.UpdateRole();
                // }
                // else{
                //     body.AddRole();
                // }
                // return Ok();
                        // var obj1=new EFModels.Role{RoleId=obj.RoleId}
                     
            //         var Roleobj=new EFModels.Role{RoleId=obj.RoleId,RoleName=obj.RoleName};

            
            //     if(await _context.Role.AnyAsync(e=>e.RoleId==Roleobj.RoleId))
            //     {
            //     var  user =  _context.Role.FirstOrDefault(e=>e.RoleId==Roleobj.RoleId);
            //         Console.WriteLine("OOOO");   
            //         user.RoleName=Roleobj.RoleName;
            //         await _context.SaveChangesAsync();
            //         return Ok(user);       
            //     }
            //   else
            //   {
            // _context.Role.AddAsync(Roleobj); 
            // _context.SaveChangesAsync();
            // return Ok(Roleobj);
         
            //         }
            
            
    //    return Ok();
    //    }
        // [HttpPost]
        // [Route("role/insert")]
        // public async Task<ActionResult> InsertRole(Role pro)
        // {
        //     _context.Role.Add(products);
        //     await _context.SaveChangesAsync();
 
        //     return CreatedAtAction("GetProducts", new { id = products.ProductId }, products);
        // }

//   public async Task<Role> AddRole(DtoRole r)
//         {  
            
//             var transaction = _context.Database.BeginTransaction();
//          try
//          {
             
//              await _context.Role.AddAsync(r);
//             await _context.SaveChangesAsync();
//             Console.WriteLine(r.RoleName);
//              transaction.Commit();
//         }
//         catch (Exception ex)
//         {
//             transaction.Rollback();
//             Console.WriteLine(ex);
//         }
    
//        return r;
//    }


        // public IActionResult Postrole(Role r1)
        // {
        //     try{
                // body.Db = Db;
                // if(body.RoleId.HasValue){
                //     body.UpdateRole();
                // }
                // else{
                //     body.AddRole();
                // }
        //         AddRole(r1);
                
        //         return Ok(r1);
        //     }
        //     catch(Exception e){
        //         Console.WriteLine(e.Message);
        //         return BadRequest();
        //     }
        // }

        

        // [HttpPost]
        // [Route("permission/update")]
        // public IActionResult Postpermission([FromBody]Permission body)
        // {
        //     try{
        //         body.Db = Db;
        //         if(body.PermissionId!=0){
        //             body.UpdatePermission();
        //         }
        //         else{
        //             body.AddPermission();
        //         }
        //         return Ok();
        //     }
        //     catch(Exception e){
        //         Console.WriteLine(e.Message);
        //         return BadRequest();
        //     }
        // }

//    [HttpPost]
//         [Route("permission/update")]
      
  
//         public  async Task<IActionResult> Postpermission(EFModels.Permission obj)
//         {       
//                 if(await _context.Permission.AnyAsync(e=>e.PermissionId==obj.PermissionId))
//                 {
//                 var  user =  _context.Permission.FirstOrDefault(e=>e.PermissionId==obj.PermissionId);
//                     Console.WriteLine("OOOO");   
//                     user.PermissionName=obj.PermissionName;
//                     await _context.SaveChangesAsync();
//                     return Ok(user);
                     
//                 }
//               else
//                     {
                    
//                          _context.Permission.AddAsync(obj);
                         
//             _context.SaveChangesAsync();
//             return Ok(obj);
         
//                     }
        
            
            
//        }







        // [HttpDelete]
        // [Route("role/{role_id}/delete")]
        // public IActionResult Deleterole(int role_id)
        // {
        //     Role query = new Role(Db);
        //     query.RoleId = role_id;
        //     if(query.Deleterole()==1)
        //     {
        //          return Ok();
        //     }
        //     else{
        //         return BadRequest();
        //     }
        // }
        // [HttpDelete]
        // [Route("permission/{permission_id}/delete")]
        // public IActionResult Deletepermission(int permission_id)
        // {
        //     Permission query = new Permission(Db);
        //     query.PermissionId = permission_id;
        //     if(query.DeletePermission()==1)
        //     {
        //          return Ok();
        //     }
        //     else{
        //         return BadRequest();
        //     }
            
        // }
      
        //  [HttpDelete]
        //  [Route("role/{role_id}/delete")]
        //  public async Task<Models.Role> Deleterole(int role_id)
        // {
           
            // var transaction =  _context.Database.BeginTransaction();
            //  try
            // {
            //     if(!await _context.RoleToPermission.AnyAsync(e=>e.RoleId==role_id))
            //     {
            //     var role1=_context.Role.FirstOrDefault(e=>e.RoleId==role_id);
                    
            //    _context.Role.Remove(role1);
            //     _context.SaveChanges();
            //     transaction.Commit();
            //     return Ok();

            //      }
            //      else{
            //          return BadRequest();
            //      }
            // }
                
            
            // catch (Exception ex)
            // {
            //     transaction.Rollback();
            //     Console.WriteLine(ex);
            //     return BadRequest();
            // }
            // return Ok();
        // }

  [HttpDelete]
         [Route("permission/{permission_id}/delete")]
         public async Task<IActionResult> DeletePerm(int permission_id)
        {
           
            var transaction =  _context.Database.BeginTransaction();
             try
            {
                if(!await _context.RoleToPermission.AnyAsync(e=>e.PermissionId==permission_id))
                {
                var perm1=_context.Permission.FirstOrDefault(e=>e.PermissionId==permission_id);
                    
               _context.Permission.Remove(perm1);
                _context.SaveChanges();
                transaction.Commit();
                return Ok();

                 }
                  else{
                     return BadRequest();
                 }
            }
                
            
            catch (Exception ex)
            {
                transaction.Rollback();
                Console.WriteLine(ex);
                return BadRequest();
            }
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