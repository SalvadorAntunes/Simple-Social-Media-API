using Microsoft.EntityFrameworkCore;
using SocialMediaAPI.Data;
using SocialMediaAPI.Exceptions;
using SocialMediaAPI.Models;

namespace SocialMediaAPI.Services
{
    public class LikeService(AppDbContext context) : ILikeService
    {
        public async Task<bool> AddLikeAsync(Like request)
        {
            var userExists = await context.Users.AnyAsync(u => u.Id == request.UserId);
            if (!userExists)
                throw new NotFoundException("User not found");

            var postExists = await context.Posts.AnyAsync(p => p.Id == request.PostId);
            if (!postExists)
                throw new NotFoundException("Post not found");

            var alreadyLiked = await context.Likes
                .AnyAsync(l => l.UserId == request.UserId && l.PostId == request.PostId);
            if (alreadyLiked)
                throw new ConflictException("Post already liked by this user");

            context.Likes.Add(new Like { UserId = request.UserId, PostId = request.PostId });
            await context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> RemoveLikeAsync(Like request)
        {
            var like = await context.Likes
                .FirstOrDefaultAsync(l => l.UserId == request.UserId && l.PostId == request.PostId);

            if (like is null)
                throw new NotFoundException("Like not found");

            context.Likes.Remove(like);
            await context.SaveChangesAsync();

            return true;
        }
    }
}
