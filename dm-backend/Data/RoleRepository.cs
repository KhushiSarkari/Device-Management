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
namespace dm_backend.data
{
    public class RoleRepository : IRoleRepository
    {
           private readonly EFDbContext _context ;
           
        public RoleRepository(EFDbContext ef): base(ef)
        {
            // Db = db;
            _context = ef ;
            //  _repo = repo;

        }
        // public AppDb Db { get; }
         public async Task<Models.Permission> PostPerm(Models.Permission data1){

                if(data1.PermissionId.HasValue)
            {            
            
                if(await _context.Permission.AnyAsync(e=>e.PermissionId==data1.PermissionId) )
                {
                    var  user =  _context.Permission.FirstOrDefault(e=>e.PermissionId==data1.PermissionId.Value);
                    user.PermissionName=data1.PermissionName;
                   
                }
                else{
                    return BadRequest();
                }
            }
              else
               {
                   var Permobj=new EFModels.Permission { PermissionName=data1.PermissionName};
                  await _context.Permission.AddAsync(Permobj);
              
                }
             await _context.SaveChangesAsync();
               return Ok();
         }



         
      public async  Task<Models.Role> Deleterole(int role_id){
        var transaction =  _context.Database.BeginTransaction();
             try
            {
                if(!await _context.RoleToPermission.AnyAsync(e=>e.RoleId==role_id))
                {
                var role1=_context.Role.FirstOrDefault(e=>e.RoleId==role_id);
                    
               _context.Role.Remove(role1);
                _context.SaveChanges();
                transaction.Commit();
                // return Ok();

                 }
                 else{
                    //  return BadRequest();
                 }
            }
                
            
            catch (Exception ex)
            {
                transaction.Rollback();
                Console.WriteLine(ex);
                //return BadRequest();
            }          
       

       
       }


}
}