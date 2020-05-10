using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using dm_backend.EFModels;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace dm_backend.Utilities
{
    public class Token
    {

    public IConfiguration _config;
        public Token(IConfiguration config)
        {
            _config=config;
        }
        public string createToken(User usertorepo,string role){
               var claims = new List<Claim>();

            claims.Add(new Claim(ClaimTypes.NameIdentifier, usertorepo.UserId.ToString()));
            claims.Add(new Claim(ClaimTypes.Name, usertorepo.Email, usertorepo.Email));
            claims.Add(new Claim(ClaimTypes.Role, role));
           

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
           
            var Result =  tokenhandler.WriteToken(token);
            Console.WriteLine(Result);
            return Result;
        }
    }
}