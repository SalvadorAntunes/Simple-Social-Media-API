using SocialMediaAPI.Data;
using SocialMediaAPI.Dtos;
using SocialMediaAPI.Models;

namespace SocialMediaAPI.Services
{

    public class LikeService(AppDbContext context) : ILikeService
    {
        public Task<bool> AddLikeAsync(Like request)
        {
            var user = context.Users.Where(u => u.Id == request.UserId).FirstOrDefault();
            if (user == null)
            {
                return Task.FromResult(false);
            }

            var post = context.Posts.Where(p => p.Id == request.PostId).FirstOrDefault();
            if (post == null)
            {
                return Task.FromResult(false);
            }

            

            return Task.FromResult(true);
        }

            

        public Task<bool> RemoveLikeAsync(Like request)
        {
            var user = context.Users.Where(u => u.Id == request.UserId).FirstOrDefault();
            if (user == null)
            {
                return Task.FromResult(false);
            }

            var post = context.Posts.Where(p => p.Id == request.PostId).FirstOrDefault();
            if (post == null)
            {
                return Task.FromResult(false);
            }

            return Task.FromResult(true);
        }
    }
}
