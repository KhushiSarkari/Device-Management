using dm_backend.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace dm_backend.Data
{
    public interface IUserRepository
    {
         List<User> GetAllUsers(string tosearch);
         User GetOneUser(int user_id);
         void PostUser(Models.User item); 
         void UpdateUser(int user_id, [FromBody]Models.User body);
         void DeleteUser(int user_id);
    }
}