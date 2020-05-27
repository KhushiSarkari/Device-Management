using System.Threading.Tasks;
namespace dm_backend.Data
{
    public interface IRoleRepository
     {

         //Task<RolePermission> PostPerm(Models.Permission data1);
         Task<EFModels.Role> Deleterole(int role_id);
         Task<EFModels.Permission> PostPerm(Models.Permission data1);

        
     }
 }
