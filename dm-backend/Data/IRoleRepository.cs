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

//  Task<RolePermission> PostPerm(Models.Permission data1);
         Task<Models.Role> Deleterole(int role_id);

         Task<Models.Permission> PostPerm(Models.Permission data1);

        
     }
 }
