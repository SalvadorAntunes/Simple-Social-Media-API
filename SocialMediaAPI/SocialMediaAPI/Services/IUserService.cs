using SocialMediaAPI.Dtos;
using SocialMediaAPI.Models;

namespace SocialMediaAPI.Services
{
    public interface IUserService
    {
        Task<List<UserResponse>> GetUsersAsync();

        Task<UserResponse?> GetUserByIdAsync(int id);

        Task<User> CreateUserAsync(User user);

        Task<bool> UpdateUserAsync(int id, User user);

        Task<bool> DeleteUserAsync(int id);
    }
}
