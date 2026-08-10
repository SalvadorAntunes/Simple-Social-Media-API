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
                Posts = u.Posts,
                LikedPosts = u.LikedPosts
            }).ToListAsync();

        public async Task<UserResponse?> GetUserByIdAsync(int id)
            => await context.Users.Where(u => u.Id == id).
            Select(u => new UserResponse
            {
                Username = u.Username,
                UName = u.UName,
                Posts = u.Posts,
                LikedPosts = u.LikedPosts
            }).FirstOrDefaultAsync();

        public async Task<User> CreateUserAsync(CreateUpdateUserRequest user)
        {
            var newUser = new User
            {
                Username = user.Username,
                Email = user.Email,
                UName = user.UName,
                Password = user.Password,
                DateOfBirth = user.DateOfBirth
            };
            context.Users.Add(newUser);
            await context.SaveChangesAsync();
            return newUser;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await context.Users.FindAsync(id);
            if (user is null)
                return false;

            context.Users.Remove(user);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateUserAsync(int id, CreateUpdateUserRequest user)
        {
            var existingUser = await context.Users.FindAsync(id);
            if (existingUser is null)
                return false;

            existingUser.Username = user.Username;
            existingUser.Email = user.Email;
            existingUser.UName = user.UName;
            existingUser.Password = user.Password;
            existingUser.DateOfBirth = user.DateOfBirth;

            await context.SaveChangesAsync();
            return true;
        }
    }
}
