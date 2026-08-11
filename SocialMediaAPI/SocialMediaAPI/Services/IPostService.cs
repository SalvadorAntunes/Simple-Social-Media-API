using SocialMediaAPI.Dtos;
using SocialMediaAPI.Models;

namespace SocialMediaAPI.Services
{
    public interface IPostService
    {
        Task<List<PostResponse>> GetPostsAsync();

        Task<PostResponse?> GetPostByIdAsync(int id);

        Task<PostResponse> CreatePostAsync(CreatePostRequest user);

        Task<bool> DeletePostAsync(int id);
    }
}
