using System.Linq;
using api.Dto;
using api.Entites;
using api.Extensions;
using AutoMapper;

namespace api.Helpers
{
    public class AutoMapperProfiles:Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AppUser,MemberDto>()
            .ForMember(dest=>dest.PhotoUrl,opt=>opt.MapFrom(
                src=>src.Photos.FirstOrDefault(x=>x.IsMain).Url)).
                ForMember(dest=>dest.Age,opt=>opt.MapFrom(src=>src.DateOfBirth.CalculateAge()));
            CreateMap<Photo,PhotosDto>();
            CreateMap<MemberUpdateDto,AppUser>();
            CreateMap<RegisterDto,AppUser>();
        }
        
    }
}