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
using Microsoft.AspNetCore.Identity;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class AccountController : BaseApiController
    {


        private readonly ITokenServices _tokenServices;
        private readonly IMapper _mapper;
        public readonly UserManager<AppUser> _userManager ;
        public  readonly SignInManager<AppUser> _signInManager ;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ITokenServices tokenServices, IMapper mapper)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _tokenServices = tokenServices;
            _mapper = mapper;

        }
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> register(RegisterDto registerDto)
        {
            if (await UserExist(registerDto.Username)) return BadRequest("User Is Taken");
            var user = _mapper.Map<AppUser>(registerDto);
            // using var hmac = new HMACSHA512();

            user.UserName = registerDto.Username.ToLower();
            //   user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password));
            //   user.PasswordSalt = hmac.Key;
            
            var result= await _userManager.CreateAsync(user,registerDto.Password);
            if(!result.Succeeded) return BadRequest(result.Errors);
            var roleResult= await _userManager.AddToRoleAsync(user,"Member");
            if(!roleResult.Succeeded) return BadRequest(result.Errors);
            return new UserDto
            {
                UserName = user.UserName,
                token = await _tokenServices.CreateToken(user),
                KnownAs = user.KnownAs,
                Gender = user.Gender
            };
        }
        private async Task<bool> UserExist(string username)
        {
            return await _userManager.Users.AnyAsync(x => x.UserName == username.ToLower());
        }
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {


            var user = await _userManager.Users
            .Include(p => p.Photos)
            .SingleOrDefaultAsync(x => x.UserName == loginDto.Username.ToLower());
            if (user == null) return Unauthorized("invalid Username !");
            // // using var hmac = new HMACSHA512(user.PasswordSalt);
            // Console.WriteLine(loginDto.Password);
            // var ComputeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));
            // for (int i = 0; i < ComputeHash.Length; i++)
            // {
            //     if (ComputeHash[i] != user.PasswordHash[i]) return Unauthorized("invalid password");
            // }
                var result= await _signInManager.CheckPasswordSignInAsync
                (user,loginDto.Password,false);
                if(!result.Succeeded) return Unauthorized();
            return new UserDto
            {
                UserName = user.UserName,
                token = await _tokenServices.CreateToken(user),
                PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url,
                KnownAs = user.KnownAs,
                Gender = user.Gender
            };

        }
    }

}