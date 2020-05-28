
 using dm_backend.EFModels;
using System.Threading.Tasks;
using System.Linq;
using System.Threading.Tasks;
using dm_backend.Data;
using dm_backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace dm_backend.Data
{
    public interface IRoleRepository
     {

         
         Task<EFModels.Role> Deleterole(int role_id);
         Task<EFModels.Permission> PostPerm(Models.Permission data1);
        Task<EFModels.Role> Postrole(Models.Role obj);
        Task<EFModels.Permission> DeletePerm(int permission_id);
        Task<Models.RolePermission> getAllRoles();
        Task<EFModels.RoleToPermission> UpdateRoles(Models.RolePermission RolePerms);
       

        

        
     }
 }
