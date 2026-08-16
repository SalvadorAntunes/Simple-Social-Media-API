using Microsoft.EntityFrameworkCore;
using SocialMediaAPI.Data;
using SocialMediaAPI.Dtos;
using SocialMediaAPI.Models;

namespace SocialMediaAPI.Services
{
    public class CommentService(AppDbContext context, IPostService postService) : ICommentService
    {
        private readonly AppDbContext _context = context;
        private readonly IPostService _postService = postService;
    
        public async Task<List<CommentResponse>> GetCommentsAsync()
            => (await context.Comments.ToListAsync()).Select(c => new CommentResponse
            {
                PostCommented = c.PostCommented,
                CommentItself = _postService.GetPostByIdAsync(c.Id).Result,
                IdNavigation = c.IdNavigation,
                PostCommentedNavigation = c.PostCommentedNavigation
            }).ToList();

        public async Task<CommentResponse?> GetCommentByIdAsync(int id)
            => (await context.Comments.Where(c => c.Id == id).ToListAsync()).Select(c => new CommentResponse
            {
                PostCommented = c.PostCommented,
                CommentItself = _postService.GetPostByIdAsync(c.Id).Result,
                IdNavigation = c.IdNavigation,
                PostCommentedNavigation = c.PostCommentedNavigation
            }).FirstOrDefault();

        public async Task<bool> DeleteCommentAsync(int id)
        {
            var comment = await _context.Comments.FirstOrDefaultAsync(c => c.Id == id);

            if (comment is null)
                return false;

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();

            _postService.DeletePostAsync(id).Wait();

            return true;
        }

        public async Task<Comment> CreateCommentAsync(CreateCommentRequest request)
        {
            var post = request.PostInfo;

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

            var newComment = new Comment
            {

                Id = newPost.Id,
                PostCommented = request.PostCommented
            };
            context.Comments.Add(newComment);
            await context.SaveChangesAsync(); 
            return newComment;
        }
      
        
    }
}
