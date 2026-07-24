using SocialMediaAPI.Models;

namespace SocialMediaAPI.Services
{
    public interface IUserService
    {
        Task<List<User>> GetUsersAsync();

        Task<User?> GetUserByIdAsync(int id);

        Task<User> AddUserAsync(User user);

        Task<bool> UpdateUserAsync(int id, User user);

        Task<bool> DeleteUserAsync(int id);
    }
}
