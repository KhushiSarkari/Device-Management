using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using dm_backend.Data;
using dm_backend.EFModels;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using Microsoft.EntityFrameworkCore.Internal;
using dm_backend.Utilities;
using Newtonsoft.Json;

namespace dm_backend.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly EFDbContext _context;
        public IAuthRepository _repo;
        public IConfiguration _config;
        private int userforlog;

        public AuthController(IAuthRepository repo, IConfiguration config, EFDbContext context)
        {
            _context = context;
            _repo = repo;
            _config = config;
        }

        [HttpPost("register")]

        public async Task<IActionResult> Register(Registration userforreg)
        {

            userforreg.Email = userforreg.Email.ToLower();
        
             if (await _repo.UserExists(userforreg.Email))
             {
                 return Ok(new { Result = "AlreadyExists" });
             }

            var userTocreate = new User
            {
                Email = userforreg.Email,
                FirstName =userforreg.FirstName,
                LastName=userforreg.LastName,
            };
           
            var createdUser = await _repo.Register(userTocreate, userforreg.Password);
             var result1=  JsonConvert.SerializeObject(createdUser, Formatting.None,
                        new JsonSerializerSettings()
                        { 
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                        });

            return Ok(new { Result = result1});
        }

        [HttpPost("Reset")]
        public async Task<IActionResult> FrogotPassword(ResetPassword rp)
        {
            rp.Email = rp.Email.ToLower();
            if (!await _repo.UserExists(rp.Email))
                return BadRequest("Not Exist");

         var obj =   new SendEmail(_context).Send_Email(rp.Email);
            return StatusCode(201);


        }
        [HttpPost("Reset/setpassword")]

        public async Task<IActionResult> SetPassword(ResetPassword rp)
        {
            byte[] passwordHash, passwordSalt;
            Console.WriteLine("setpass");
            _repo.CreatePasswordHash(rp.Password, out passwordHash, out passwordSalt);


            Console.WriteLine(passwordSalt);
            var user = _context.User.First(a => a.Guid == rp.Guid);
            user.Hashpassword = passwordHash;
            user.Saltpassword = passwordSalt;
            Console.WriteLine("done !");
            _context.SaveChanges();
            return Ok(new { Result = "Done" });



        }

      

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto Userforlog)
        {
            var usertorepo = await _repo.Login(Userforlog.Email, Userforlog.Password);
            if (usertorepo == null)
                return Unauthorized();


            //linq qwery
            var entryPoint = (from us in _context.User
                              join rl in _context.UserToRole on us.UserId equals rl.UserId
                              join r in _context.Role on rl.RoleId equals r.RoleId
                              where us.UserId == usertorepo.UserId
                              select new
                              {
                                  Role = r.RoleName
                              }).ToList();

            var claims = new List<Claim>();

            claims.Add(new Claim(ClaimTypes.NameIdentifier, usertorepo.UserId.ToString()));
            claims.Add(new Claim(ClaimTypes.Name, usertorepo.Email, usertorepo.Email));
            foreach (var role in entryPoint)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.Role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("Appsetting:Token").Value));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = cred
            };

            var tokenhandler = new JwtSecurityTokenHandler();
            var token = tokenhandler.CreateToken(tokenDescriptor);
            Console.WriteLine(token);

            var result = new RedirectResult("http://127.0.0.1:1234/dashboard.html?token=" + tokenhandler.WriteToken(token) + "&id=" + usertorepo.UserId.ToString());
            return result;
        }
    }
   
}