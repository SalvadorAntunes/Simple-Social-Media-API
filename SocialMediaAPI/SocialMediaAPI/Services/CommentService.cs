using Microsoft.EntityFrameworkCore;
using SocialMediaAPI.Data;
using SocialMediaAPI.Dtos;
using SocialMediaAPI.Exceptions;
using SocialMediaAPI.Models;

namespace SocialMediaAPI.Services
{
    public class CommentService(AppDbContext context, IPostService postService) : ICommentService
    {
        public async Task<List<CommentResponse>> GetCommentsAsync()
        {
            var comments = await context.Comments.AsNoTracking().ToListAsync();

            var commentIds = comments.Select(c => c.Id).ToList();
            var posts = (await postService.GetPostsByIdsAsync(commentIds)).ToDictionary(p => p.Id);

            return comments.Select(c => new CommentResponse
            {
                PostCommented = c.PostCommented,
                CommentItself = posts[c.Id]
            }).ToList();
        }

        public async Task<CommentResponse?> GetCommentByIdAsync(int id)
        {
            var comment = await context.Comments.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);

            if (comment is null)
                return null;

            return new CommentResponse
            {
                PostCommented = comment.PostCommented,
                CommentItself = (await postService.GetPostByIdAsync(id))!
            };
        }

        public async Task<CommentResponse> CreateCommentAsync(CreateCommentRequest request)
        {
            var post = request.PostInfo;

            var userExists = await context.Users.AnyAsync(u => u.Id == post.UserId);
            if (!userExists)
                throw new NotFoundException("User not found");

            var commentedPostExists = await context.Posts.AnyAsync(p => p.Id == request.PostCommented);
            if (!commentedPostExists)
                throw new NotFoundException("Post not found");

            // The comment and the post that holds its content are saved in a single
            // SaveChanges, so either both are created or neither is.
            var newComment = new Comment
            {
                PostCommented = request.PostCommented,
                IdNavigation = new Post
                {
                    UserId = post.UserId,
                    PostDate = DateTime.UtcNow,
                    Text = post.Text
                }
            };

            context.Comments.Add(newComment);
            await context.SaveChangesAsync();

            return new CommentResponse
            {
                PostCommented = newComment.PostCommented,
                CommentItself = (await postService.GetPostByIdAsync(newComment.Id))!
            };
        }

        public async Task<bool> DeleteCommentAsync(int id)
        {
            var commentExists = await context.Comments.AnyAsync(c => c.Id == id);

            if (!commentExists)
                return false;

            // A comment is a post, so deleting the post removes the comment,
            // its likes and its replies in one transaction.
            return await postService.DeletePostAsync(id);
        }
    }
}
