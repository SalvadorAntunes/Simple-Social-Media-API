using Microsoft.EntityFrameworkCore;
using SocialMediaAPI.Data;
using SocialMediaAPI.Dtos;
using SocialMediaAPI.Models;

namespace SocialMediaAPI.Services
{
    public class UserService(AppDbContext context) : IUserService
    {
        public async Task<List<UserResponse>> GetUsersAsync()
            => await context.Users.Select(u => new UserResponse
            {
                Username = u.Username,
                UName = u.UName,
                PostsNavigation = u.PostsNavigation,
                Posts = u.Posts
            }).ToListAsync();

        public async Task<UserResponse?> GetUserByIdAsync(int id)
            => await context.Users.Where(u => u.Id == id).
            Select(u => new UserResponse
            {
                Username = u.Username,
                UName = u.UName,
                PostsNavigation = u.PostsNavigation,
                Posts = u.Posts
            }).FirstOrDefaultAsync();

        public Task<User> CreateUserAsync(User user)
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
