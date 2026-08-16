using SocialMediaAPI.Dtos;
using SocialMediaAPI.Models;

namespace SocialMediaAPI.Services
{
    public interface ILikeService
    {
        Task<List<LikeResponse>> GetLikesAsync();
    }
}
