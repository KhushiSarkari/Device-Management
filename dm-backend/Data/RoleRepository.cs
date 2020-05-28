
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
    public class RoleRepository : ControllerBase , IRoleRepository
    {
           private readonly EFDbContext _context ;
           
        public RoleRepository(EFDbContext ef) 
        {
            _context = ef ;

        }

public async Task<Models.RolePermission> getAllRoles(){
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
               
                return rp;
}

         public async Task<EFModels.Permission> PostPerm(Models.Permission data1)
         {

                var transaction =  _context.Database.BeginTransaction();
             try
            {  
                if(data1.PermissionId.HasValue)
                {            
                    if(await _context.Permission.AnyAsync(e=>e.PermissionId==data1.PermissionId) )
                    {
                       var user =  _context.Permission.FirstOrDefault(e=>e.PermissionId==data1.PermissionId.Value);
                        user.PermissionName=data1.PermissionName;
                         await _context.SaveChangesAsync();
                         transaction.Commit();
                        return user ;
                    }
                    else
                    {
                            return null;
                    }
                }
              else
               {
                   var Permobj=new EFModels.Permission { PermissionName=data1.PermissionName};
                  await _context.Permission.AddAsync(Permobj);
                  await _context.SaveChangesAsync();
                  transaction.Commit();
                  return Permobj;
              
               }
                 }
             catch (Exception ex)
            {
                transaction.Rollback();
                Console.WriteLine(ex);
                return null;
            }          
       
            
         }

         

          public  async Task<EFModels.RoleToPermission> UpdateRoles(Models.RolePermission RolePerms)
        {
            
            var transaction =  _context.Database.BeginTransaction();
             try
            {            
       foreach (Models.Role roleobj in RolePerms.Roles)
               {

                     var x=(_context.Role.FirstOrDefault(e=>e.RoleName==roleobj.RoleName).RoleId);
                     _context.RoleToPermission.RemoveRange(_context.RoleToPermission.Where(e=> e.RoleId==x));
                 _context.SaveChanges();

              
                 if(roleobj.Permissions!=null){
                     
                    foreach(Models.Permission permobj in roleobj.Permissions)
                    {
                         var perm_id=_context.Permission.FirstOrDefault(e=>e.PermissionName==permobj.PermissionName).PermissionId;
                         var role_id1=_context.Role.FirstOrDefault(e=>e.RoleName==roleobj.RoleName).RoleId;
                       var perm_int=new EFModels.RoleToPermission();
                       perm_int.RoleId= role_id1;
                       perm_int.PermissionId=perm_id;
                           _context.RoleToPermission.Add(perm_int);
                          _context.SaveChanges();
                         

                                   }
                 }
                
}
 transaction.Commit();
            }
              catch (Exception ex)
            {
                transaction.Rollback();
                Console.WriteLine(ex);
                return null;
            }      
           

               return null;
        }

   public async Task<EFModels.Role> Postrole(Models.Role obj)
   {
       var transaction =  _context.Database.BeginTransaction();
             try
            {
        if(obj.RoleId.HasValue)
                {            
                    if(await _context.Role.AnyAsync(e=>e.RoleId==obj.RoleId) )
                    {
                       var user =  _context.Role.FirstOrDefault(e=>e.RoleId==obj.RoleId.Value);
                        user.RoleName=obj.RoleName;
                         await _context.SaveChangesAsync();
                         transaction.Commit();
                        return user ;
                    }
                    else
                    {
                            return null;
                    }
                }
              else
               {
                   var Permobj=new EFModels.Role { RoleName=obj.RoleName};
                  await _context.Role.AddAsync(Permobj);
                  await _context.SaveChangesAsync();
                  transaction.Commit();
                  return Permobj;
              
               }
                }
             catch (Exception ex)
            {
                transaction.Rollback();
                Console.WriteLine(ex);
                return null;
            }      
   }
    
      public async  Task<EFModels.Role> Deleterole(int role_id){
        var transaction =  _context.Database.BeginTransaction();
             try
            {
                if(!await _context.RoleToPermission.AnyAsync(e=>e.RoleId==role_id))
                {
                var role1=_context.Role.FirstOrDefault(e=>e.RoleId==role_id);
                    
               _context.Role.Remove(role1);
                _context.SaveChanges();
                transaction.Commit();
                return role1;

                 }
                else{return null;}
            }
             catch (Exception ex)
            {
                transaction.Rollback();
                Console.WriteLine(ex);
                return null;
            }          
       

       
       }

       public async Task<EFModels.Permission> DeletePerm(int permission_id)
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
                return perm1;

                 }
                else{return null;}
            }
             catch (Exception ex)
            {
                transaction.Rollback();
                Console.WriteLine(ex);
                return null;
            }          
       

       }

      
    }
}