using Microsoft.EntityFrameworkCore;
using SocialMediaAPI.Data;
using SocialMediaAPI.Dtos;
using SocialMediaAPI.Models;

namespace SocialMediaAPI.Services
{
    public class PostService(AppDbContext context) : IPostService
    {  
        public async Task<List<PostResponse>> GetPostsAsync()
            => await context.Posts.Select(p => new PostResponse
            {
                Id = p.Id,
                UserId = p.UserId,
                AuthorUsername = p.User.Username,
                PostDate = p.PostDate,
                Text = p.Text,
                CommentIdNavigation = p.CommentIdNavigation,
                CommentPostCommentedNavigations = p.CommentPostCommentedNavigations,
                User = p.User,
                Users = p.Users
            }).ToListAsync();

        public Task<PostResponse?> GetPostByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Post> CreatePostAsync(CreatePostRequest user)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeletePostAsync(int id)
        {
            throw new NotImplementedException();
        }

        
    }
}
