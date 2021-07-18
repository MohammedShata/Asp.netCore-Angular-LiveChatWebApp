using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dto;
using api.Entites;
using api.Extensions;
using api.Helpers;
using api.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace api.Data
{
    public class LikeRepository : ILikesRepository
    {
        private readonly DataContext _context;
        public LikeRepository(DataContext context)
        {
            _context=context;
        }
        public async Task<UserLike> GetUserLike(int sourceUserId, int LikedUserId)
        {
           
          return await _context.Likes.FindAsync(sourceUserId,LikedUserId);
        }

        public async Task<PagedList<LikeDto>> GetUserLikes(LikesParams likesParams)
        {
           var user =_context.Users.OrderBy(u=>u.UserName).AsQueryable();
           var likes=_context.Likes.AsQueryable();
           if(likesParams.Predicate=="liked")
           {
               likes=likes.Where(like=>like.SourceUserId==likesParams.UserId);
               user=likes.Select(like=>like.LikedUser);
           }
           if(likesParams.Predicate=="likedBy")
           {
               likes=likes.Where(like=>like.LikedUserId==likesParams.UserId);
               user=likes.Select(like=>like.SourceUser);
           }

           var LikedUser= user.Select(user=> new LikeDto
           {
               Username=user.UserName,
               KnownAs=user.KnownAs,
               Age=user.DateOfBirth.CalculateAge(),
               PhotoUrl=user.Photos.FirstOrDefault(x=>x.IsMain).Url,
               City=user.City,
               Id=user.Id

           });
           return await PagedList<LikeDto>.CreateAsyc(
               LikedUser,likesParams.PageNumber,likesParams.PageSize);
        }

        public async Task<AppUser> GetUserWithLikes(int userId)
        {
            return await _context.Users.Include(x=>x.LikedUsers).FirstOrDefaultAsync(x=>x.Id==userId);
        }
    }
}