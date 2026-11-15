using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using SocialMediaAPI.Data;
using SocialMediaAPI.Dtos;
using SocialMediaAPI.Exceptions;
using SocialMediaAPI.Models;

namespace SocialMediaAPI.Services
{
    public class PostService(AppDbContext context) : IPostService
    {
        // One projection shared by every query, so posts always come back in the same shape.
        private static readonly Expression<Func<Post, PostResponse>> ToResponse = p => new PostResponse
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
        };

        public async Task<List<PostResponse>> GetPostsAsync()
            => await context.Posts.AsNoTracking()
                .Select(ToResponse)
                .ToListAsync();

        public async Task<List<PostResponse>> GetPostsByIdsAsync(List<int> ids)
            => await context.Posts.AsNoTracking()
                .Where(p => ids.Contains(p.Id))
                .Select(ToResponse)
                .ToListAsync();

        public async Task<PostResponse?> GetPostByIdAsync(int id)
            => await context.Posts.AsNoTracking()
                .Where(p => p.Id == id)
                .Select(ToResponse)
                .FirstOrDefaultAsync();

        public async Task<PostResponse> CreatePostAsync(CreatePostRequest post)
        {
            var userExists = await context.Users.AnyAsync(u => u.Id == post.UserId);

            if (!userExists)
                throw new NotFoundException("User not found");

            var newPost = new Post
            {
                UserId = post.UserId,
                PostDate = DateTime.UtcNow,
                Text = post.Text
            };

            context.Posts.Add(newPost);
            await context.SaveChangesAsync();

            return (await GetPostByIdAsync(newPost.Id))!;
        }

        public async Task<bool> DeletePostAsync(int id)
        {
            var postExists = await context.Posts.AnyAsync(p => p.Id == id);
            if (!postExists)
                return false;

            await using var transaction = await context.Database.BeginTransactionAsync();
            await DeletePostsWithRepliesAsync([id]);
            await transaction.CommitAsync();

            return true;
        }

        // Deletes the given posts together with everything that depends on them:
        // their likes and every reply below them, at any depth.
        // Runs several statements, so the caller must wrap it in a transaction.
        public async Task DeletePostsWithRepliesAsync(List<int> postIds)
        {
            var ids = postIds.ToList();

            // Walk down the comment tree one level at a time.
            var level = postIds;
            while (level.Count > 0)
            {
                var parents = level;
                level = await context.Comments
                    .Where(c => parents.Contains(c.PostCommented) && !ids.Contains(c.Id))
                    .Select(c => c.Id)
                    .ToListAsync();
                ids.AddRange(level);
            }

            await context.Likes
                .Where(l => ids.Contains(l.PostId))
                .ExecuteDeleteAsync();
            await context.Comments
                .Where(c => ids.Contains(c.Id) || ids.Contains(c.PostCommented))
                .ExecuteDeleteAsync();
            await context.Posts
                .Where(p => ids.Contains(p.Id))
                .ExecuteDeleteAsync();
        }
    }
}
