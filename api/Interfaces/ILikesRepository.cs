using System.Collections.Generic;
using System.Threading.Tasks;
using api.Dto;
using api.Entites;
using api.Helpers;

namespace api.Interfaces
{
    public interface ILikesRepository
    {
        Task<UserLike> GetUserLike(int sourceUserId,int LikedUserId);
        Task<AppUser> GetUserWithLikes(int userId);
        Task<PagedList<LikeDto>> GetUserLikes(LikesParams LikesParams);
    }
}