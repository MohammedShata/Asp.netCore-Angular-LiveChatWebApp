using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using api.Entites;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using api.Dto;
using api.Interfaces;
using AutoMapper;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class AccountController : BaseApiController
    {
        
        private readonly DataContext _Context;
        private readonly ITokenServices _tokenServices;
        private readonly IMapper _mapper;

        public AccountController(DataContext Context, ITokenServices tokenServices,IMapper mapper)
        {
           
            _Context = Context;
            _tokenServices= tokenServices;
            _mapper = mapper;
           
        }
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> register(RegisterDto registerDto)
        {
            if (await UserExist(registerDto.Username)) return BadRequest("User Is Taken");
            var user= _mapper.Map<AppUser>(registerDto);
            using var hmac = new HMACSHA512();
           
              user.UserName = registerDto.Username.ToLower();
              user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password));
              user.PasswordSalt = hmac.Key;
        
            _Context.Add(user);
            await _Context.SaveChangesAsync();
            return new UserDto{
               Username=user.UserName,
               token=_tokenServices.CreateToken(user),
               KnownAs=user.KnownAs,
               Gender=user.Gender
            };
        }
        private async Task<bool> UserExist(string username)
        {
            return await _Context.Users.AnyAsync(x => x.UserName == username.ToLower());
        }
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
          

            var user = await _Context.Users.Include(p=>p.Photos).SingleOrDefaultAsync(x => x.UserName == loginDto.Username);
            if (user == null) return Unauthorized("invalid User !");
            using var hmac = new HMACSHA512(user.PasswordSalt);
            Console.WriteLine(loginDto.Password);
            var ComputeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));
            for (int i = 0; i < ComputeHash.Length; i++)
            {
                if (ComputeHash[i] != user.PasswordHash[i]) return Unauthorized("invalid password");
            }
            return new UserDto{
                Username=user.UserName,
                token=_tokenServices.CreateToken(user),
                PhotoUrl=user.Photos.FirstOrDefault(x=>x.IsMain)?.Url,
                KnownAs=user.KnownAs,
                Gender=user.Gender
            };
           
        }
    }

}