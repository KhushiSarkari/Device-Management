using System.Threading.Tasks;
using dm_backend.EFModels;
using dm_backend.Utilities;

namespace dm_backend.Data
{
    public interface IUserRepository
    {
         Task<PagedList<User>> GetUsers(UserParams userParams);
    }
}