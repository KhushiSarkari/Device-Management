using dm_backend.EFModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace dm_backend.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly EFDbContext _context;
        public AuthRepository(EFDbContext context)
        {
            _context = context;
        }

        public async Task<User> Register(User  user, string password)
        {  
            var transaction = _context.Database.BeginTransaction();
         try
         {
              byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);
            user.Hashpassword = passwordHash;
            user.Saltpassword = passwordSalt;
            user.Status =1 ;
             await _context.User.AddAsync(user);
            await _context.SaveChangesAsync();
            Console.WriteLine(user.UserId);
            var _role= new UserToRole();
            _role.UserId=user.UserId;
            _role.RoleId=1;
            await _context.UserToRole.AddAsync(_role);
            await _context.SaveChangesAsync();
             transaction.Commit();
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            Console.WriteLine(ex);
        }
    
       return user;
   }

        public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            Console.WriteLine(password);
            using var hmac = new System.Security.Cryptography.HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }

        public async Task<User> Login(string email, string password)
        {
            var user = await _context.User.FirstOrDefaultAsync(x => (x.Email == email)&&(x.Status==1));
            
            if (user == null)
            {
                return null;
            }

            if (!Verify(password, user.Hashpassword, user.Saltpassword))
                return null;
            

            return user;

        }

        private bool Verify(string password, byte[] hashpassword, byte[] saltpassword)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(saltpassword))
            {
                var computerHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

                for (int i = 0; i < computerHash.Length; i++)
                {
                    if (computerHash[i] != hashpassword[i])
                        return false;
                }
            }
            return true;
        }

        public async Task<bool> UserExists(string email)
        {
            if (await _context.User.AnyAsync(x => x.Email == email))
                return true;
            return false;
        }
    }
}
