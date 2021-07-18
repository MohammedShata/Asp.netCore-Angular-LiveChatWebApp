using api.Entites;
using api.Interfaces;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Collections.Generic;
using System.Security.Claims;
using System;

namespace api.Services
{
    public class TokenServices : ITokenServices
    {
        private readonly SymmetricSecurityKey _key;
        public TokenServices(IConfiguration config)
        {
            _key=new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));
        }
        public string CreateToken(AppUser user)
        {
            var claims=new List<Claim>
            {
              new Claim(JwtRegisteredClaimNames.NameId,user.Id.ToString()),
               new Claim(JwtRegisteredClaimNames.UniqueName,user.UserName)
            };
            var cred=new SigningCredentials(_key,SecurityAlgorithms.HmacSha512Signature);
            var tokenDescribtion=new SecurityTokenDescriptor{
                Subject=new ClaimsIdentity(claims),
                Expires=DateTime.Now.AddDays(7),
                SigningCredentials=cred
            };
            var tokenhandler=new JwtSecurityTokenHandler();
            var token=tokenhandler.CreateToken(tokenDescribtion);
            return tokenhandler.WriteToken(token);
        }
    }
}