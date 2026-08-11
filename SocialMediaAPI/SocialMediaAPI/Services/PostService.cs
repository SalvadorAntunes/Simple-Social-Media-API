using Microsoft.EntityFrameworkCore;
using SocialMediaAPI.Data;
using SocialMediaAPI.Dtos;
using SocialMediaAPI.Models;

namespace SocialMediaAPI.Services
{
    public class PostService(AppDbContext context) : IPostService
    {
        public async Task<List<PostResponse>> GetPostsAsync()
           => await context.Posts
                .Select(p => new PostResponse
                {
                    Id = p.Id,
                    UserId = p.UserId,
                    PostDate = p.PostDate,
                    Text = p.Text,
                    CommentIdNavigation = p.CommentIdNavigation,
                    CommentPostCommentedNavigations = p.CommentPostCommentedNavigations,
                    User = new UserInfo
                    {
                        Id = p.User.Id,
                        Username = p.User.Username,
                        UName = p.User.UName
                    },
                    
                    Users = p.Users.Select(u => new UserInfo
                    {
                        Id = u.Id,
                        Username = u.Username,
                        UName = u.UName
                    }).ToList()
                })
                .ToListAsync();
        

        public async Task<PostResponse?> GetPostByIdAsync(int id)
            => await context.Posts.Where(u => u.Id == id)
                .Select(p => new PostResponse
                {
                    Id = p.Id,
                    UserId = p.UserId,
                    PostDate = p.PostDate,
                    Text = p.Text,
                    CommentIdNavigation = p.CommentIdNavigation,
                    CommentPostCommentedNavigations = p.CommentPostCommentedNavigations,
                    User = new UserInfo
                    {
                        Id = p.User.Id,
                        Username = p.User.Username,
                        UName = p.User.UName
                    },

                    Users = p.Users.Select(u => new UserInfo
                    {
                        Id = u.Id,
                        Username = u.Username,
                        UName = u.UName
                    }).ToList()
                })
                .FirstOrDefaultAsync();


        public async Task<PostResponse> CreatePostAsync(CreatePostRequest post)
        {
            var userExists = await context.Users.AnyAsync(u => u.Id == post.UserId);

            if (!userExists)
                throw new Exception("User not found");

            var newPost = new Post
            {
                UserId = post.UserId,
                PostDate = DateTime.Now,
                Text = post.Text
            };

            context.Posts.Add(newPost);
            await context.SaveChangesAsync(); 
            return new PostResponse
            {
                Id = newPost.Id,
                UserId = newPost.UserId,
                PostDate = newPost.PostDate,
                Text = newPost.Text
            };
        }

        public async Task<bool> DeletePostAsync(int id)
        {
            var post = await context.Posts.Where(u => id == u.Id).FirstOrDefaultAsync();
            if (post is null)
                return false;

            context.Posts.Remove(post);
            await context.SaveChangesAsync();
            return true;
        }

        
    }
}
