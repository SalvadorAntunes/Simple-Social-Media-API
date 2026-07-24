using Microsoft.EntityFrameworkCore;
using SocialMediaAPI.Data;
using SocialMediaAPI.Models;

namespace SocialMediaAPI.Services
{
    public class UserService(AppDbContext context) : IUserService
    {
        public async Task<List<User>> GetUsersAsync()
            => await context.Users.ToListAsync();

        public async Task<User?> GetUserByIdAsync(int id)
            => await context.Users.FindAsync(id);

        public Task<User> AddUserAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteUserAsync(int id)
        {
            throw new NotImplementedException();
        }
        
        public Task<bool> UpdateUserAsync(int id, User user)
        {
            throw new NotImplementedException();
        }
    }
}
