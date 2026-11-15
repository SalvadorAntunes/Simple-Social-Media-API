using Microsoft.EntityFrameworkCore;
using SocialMediaAPI.Data;
using SocialMediaAPI.Dtos;
using SocialMediaAPI.Exceptions;
using SocialMediaAPI.Models;

namespace SocialMediaAPI.Services
{
    public class UserService(AppDbContext context, IPostService postService) : IUserService
    {
        public async Task<List<UserResponse>> GetUsersAsync()
            => await context.Users.Select(u => new UserResponse
            {
                Id = u.Id,
                Username = u.Username,
                UName = u.UName,
                Posts = u.Posts,
                LikedPosts = u.LikedPosts
            }).ToListAsync();

        public async Task<UserResponse?> GetUserByIdAsync(int id)
            => await context.Users.Where(u => u.Id == id).
            Select(u => new UserResponse
            {
                Id = u.Id,
                Username = u.Username,
                UName = u.UName,
                Posts = u.Posts,
                LikedPosts = u.LikedPosts
            }).FirstOrDefaultAsync();

        public async Task<UserResponse> CreateUserAsync(CreateUpdateUserRequest user)
        {
            await EnsureUsernameAndEmailAreFreeAsync(user, ignoreUserId: null);

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

            return new UserResponse
            {
                Id = newUser.Id,
                Username = newUser.Username,
                UName = newUser.UName
            };
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var userExists = await context.Users.AnyAsync(u => u.Id == id);
            if (!userExists)
                return false;

            // Deleting a user also removes their posts and comments (with the replies
            // below them) and their likes, all in one transaction.
            await using var transaction = await context.Database.BeginTransactionAsync();

            var postIds = await context.Posts
                .Where(p => p.UserId == id)
                .Select(p => p.Id)
                .ToListAsync();
            await postService.DeletePostsWithRepliesAsync(postIds);

            await context.Likes.Where(l => l.UserId == id).ExecuteDeleteAsync();
            await context.Users.Where(u => u.Id == id).ExecuteDeleteAsync();

            await transaction.CommitAsync();
            return true;
        }

        public async Task<bool> UpdateUserAsync(int id, CreateUpdateUserRequest user)
        {
            var existingUser = await context.Users.FindAsync(id);
            if (existingUser is null)
                return false;

            await EnsureUsernameAndEmailAreFreeAsync(user, ignoreUserId: id);

            existingUser.Username = user.Username;
            existingUser.Email = user.Email;
            existingUser.UName = user.UName;
            existingUser.Password = user.Password;
            existingUser.DateOfBirth = user.DateOfBirth;

            await context.SaveChangesAsync();
            return true;
        }

        private async Task EnsureUsernameAndEmailAreFreeAsync(CreateUpdateUserRequest user, int? ignoreUserId)
        {
            var conflicts = await context.Users
                .Where(u => u.Id != ignoreUserId && (u.Username == user.Username || u.Email == user.Email))
                .Select(u => new { u.Username, u.Email })
                .ToListAsync();

            if (conflicts.Any(u => string.Equals(u.Username, user.Username, StringComparison.OrdinalIgnoreCase)))
                throw new ConflictException("Username already in use");

            if (conflicts.Count > 0)
                throw new ConflictException("Email already in use");
        }
    }
}
