using SocialMediaAPI.Dtos;
using SocialMediaAPI.Models;

namespace SocialMediaAPI.Services
{
    public interface ILikeService
    {
        Task<bool> AddLikeAsync(Like request);
        Task<bool> RemoveLikeAsync(Like request);
    }
}
