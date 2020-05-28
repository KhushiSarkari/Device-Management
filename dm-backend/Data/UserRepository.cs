using System.Linq;
using System.Threading.Tasks;
using dm_backend.EFModels;
using dm_backend.Utilities;
using dm_backend.Utilities.Pagination;
using Microsoft.EntityFrameworkCore;

namespace dm_backend.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly EFDbContext _context ;
        public UserRepository(EFDbContext context)
        {
            _context = context;
        }
        public async Task<PagedList<User>> GetUsers(UserParams userParams)
        {
            var users = _context.User.OrderBy(e=>e.FirstName).AsQueryable();
            if(!string.IsNullOrEmpty(userParams.orderBy))
            {
                switch(userParams.orderBy)
                {
                    case "firstname" : 
                    users=users.OrderByDescending(e=>e.FirstName);
                    break;
                    case "email" :
                    users = users.OrderBy(e=>e.Email);
                    break ;
                }

            }
            return await  PageList<User>.CreateAsync(users , userParams.PageNumber , userParams.PageSize );
        }

        
    }
}