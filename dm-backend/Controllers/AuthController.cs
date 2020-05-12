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
using dm_backend.Logics;

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
            //send email on registration 
            string body ="Congratulations !<br>"+createdUser.FirstName+" "+createdUser.LastName+"<br>  Your account has been created on Device Management portal  <br> Thanks  ";
            var emailObj = new sendMail().sendNotification(createdUser.Email,body, "Registration Successfull");

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
           _repo.CreatePasswordHash(rp.Password, out passwordHash, out passwordSalt);


            var user = _context.User.FirstOrDefault(a => a.Guid == rp.Guid);
            user.Hashpassword = passwordHash;
            user.Saltpassword = passwordSalt;
            _context.SaveChanges();
            return Ok(new { Result = "Done" });

        }

   [HttpPost("loginUsingGoogle")]
   public async Task<IActionResult> loginUsingGoogle(googleLogin userto){
       Console.WriteLine("---------------");
       //  userto.Email = userto.Email.ToLower();
         Console.WriteLine(userto.Email+userto.ClientId+userto.FirstName+userto.LastName);
         if(! await _repo.UserExists(userto.Email))
         {
          var userTocreate = new User
            {
                Email = userto.Email,
                FirstName =userto.FirstName,
                LastName=userto.LastName,
            };
            var createdUser = await _repo.Register(userTocreate,userto.ClientId);
        }
       var userthis = (from us in _context.User
                   where us.Email == userto.Email
                   select us).FirstOrDefault();
    
            var entryPoint = (from us in _context.User
                              join rl in _context.UserToRole on us.UserId equals rl.UserId
                              join r in _context.Role on rl.RoleId equals r.RoleId
                              where us.UserId == userthis.UserId
                              select new
                              {
                                  Role = r.RoleName
                              }).ToList();
           
        
        var TokenReq =new TokenRequirements{UserId=userthis.UserId , Email=userthis.Email , Role =entryPoint[0].Role};
        var GetToken = new Token(_config).createToken(TokenReq);
        
    var result = new RedirectResult("http://127.0.0.1:1234/dashboard.html?token="+ GetToken + "&id=" + userthis.UserId.ToString());
            return result;

        
   }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto Userforlog,int number)
        {
            var usertorepo = await _repo.Login(Userforlog.Email, Userforlog.Password);
            if (usertorepo == null)
                return Unauthorized();


            //link query
            var entryPoint = (from us in _context.User
                              join rl in _context.UserToRole on us.UserId equals rl.UserId
                              join r in _context.Role on rl.RoleId equals r.RoleId
                              where us.UserId == usertorepo.UserId
                              select new
                              {
                                  Role = r.RoleName
                              }).ToList();
           var TokenRequ = new TokenRequirements {UserId=usertorepo.UserId , Email = usertorepo.Email ,Role=entryPoint[0].Role.ToString()};

         var token = new Token(_config).createToken(TokenRequ);

            var result = new RedirectResult("http://127.0.0.1:1234/dashboard.html?token="+ token + "&id=" + usertorepo.UserId.ToString());
            return result;
        }
    }
   
}