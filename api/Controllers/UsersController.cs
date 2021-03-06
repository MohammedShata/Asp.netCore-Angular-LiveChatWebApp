using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using api.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using api.Interfaces;
using api.Dto;
using AutoMapper;
using System.Security.Claims;
using api.Extensions;
using Microsoft.AspNetCore.Http;
using api.Helpers;

namespace api.Controllers
{
    // [ApiController]
    // [Route("api/[Controller]")]
    [Authorize]
    public class UsersController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IPhotoServices _photoServices;
        public UsersController(IUserRepository userRepository, IMapper mapper, IPhotoServices photoServices)
        {
            _photoServices = photoServices;
            _mapper = mapper;
            _userRepository = userRepository;

        }
        // [Authorize(Roles ="Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers
        ([FromQuery]UserParams userParams)
        {
            var user=await _userRepository.GetUserByUsernameAsync(User.GetUsername());
            userParams.CurrentUsername=user.UserName;
            if(string.IsNullOrEmpty(userParams.Gender))
            {
                if(userParams.Gender==user.Gender)
                 userParams.Gender= "male";
                 else{
                     userParams.Gender="female";
                 }
            }
            
            var users = await _userRepository.GetMembersAsync(userParams);
            Response.AddPaginationHeader(users.CurrentPage,users.PageSize,users.TotalCount,users.TotalPages);
            return Ok(users);
            }
        // [Authorize(Roles ="Member")]
        [HttpGet("{username}",Name="GetUser")]
        public async Task<ActionResult<MemberDto>> GetUser(string username)
        {
            return await _userRepository.GetMemberAsync(username);

        }
        [HttpPut]
        public async Task<ActionResult<MemberDto>> UpdateUser(MemberUpdateDto memberUpdateDto)
        {
            var username = User.GetUsername();
            var user = await _userRepository.GetUserByUsernameAsync(username);
            _mapper.Map(memberUpdateDto, user);
            _userRepository.Update(user);
            if (await _userRepository.SaveAllAsync()) return NoContent();
            return BadRequest("Failed to Update user");
        }

        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotosDto>> AddPhoto(IFormFile file)
        {
            var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());
        var result = await _photoServices.AddPhotoAsync(file);
        if(result.Error !=null) return BadRequest(result.Error.Message);

        var photo=new Photo{
            Url=result.SecureUrl.AbsoluteUri,
            PublicId=result.PublicId
        };
        if(user.Photos.Count == 0)
        {
            photo.IsMain=true;
        }
        user.Photos.Add(photo);
        if(await _userRepository.SaveAllAsync())
        
    {
        return CreatedAtRoute("getUser",new {username=user.UserName},_mapper.Map<PhotosDto>(photo));
    }
        return BadRequest("problem adding photo");

        }

[HttpPut("set-main-photo/{photoId}")]
public async Task<ActionResult> SetMainPhoto(int photoId)
{
    var user= await _userRepository.GetUserByUsernameAsync(User.GetUsername());
    var photo=user.Photos.FirstOrDefault(x=>x.Id==photoId);
    if(photo.IsMain) return BadRequest("This is already your main photo");
    var currentMain=user.Photos.FirstOrDefault(x=>x.IsMain);
    if(currentMain !=null) currentMain.IsMain=false;
    photo.IsMain=true;
    if(await _userRepository.SaveAllAsync())
    return NoContent();
    return BadRequest("Failed to set main photo");
}
[HttpDelete("delete-photo/{photoId}")]
public async Task<ActionResult> DeletePhoto(int photoId)
{
    var user= await _userRepository.GetUserByUsernameAsync(User.GetUsername());
    var photo=user.Photos.FirstOrDefault(x=>x.Id == photoId);

    if(photo==null) return NotFound();
    if(photo.IsMain) return BadRequest("You Can't Delete Main Photo");
    if(photo.PublicId !=null)
    {
        var result=await _photoServices.DeletePhotoAsync(photo.PublicId);
        if(result.Error!=null) return BadRequest(result.Error.Message);
    }
    user.Photos.Remove(photo);

    if(await _userRepository.SaveAllAsync()) return Ok();

    return BadRequest("Failed to Delete");
    
}


}
}