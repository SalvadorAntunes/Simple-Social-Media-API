using SocialMediaAPI.Dtos;
using SocialMediaAPI.Models;


namespace SocialMediaAPI.Services
{
    public interface ICommentService
    {
        Task<List<CommentResponse>> GetCommentsAsync();

        Task<CommentResponse?> GetCommentByIdAsync(int id);

        Task<Comment> CreateCommentAsync(CreateCommentRequest request);

        Task<bool> DeleteCommentAsync(int id);
    }
}
