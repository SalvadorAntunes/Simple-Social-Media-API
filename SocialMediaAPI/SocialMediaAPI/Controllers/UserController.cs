using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialMediaAPI.Dtos;
using SocialMediaAPI.Models;
using SocialMediaAPI.Services;

namespace SocialMediaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IUserService service) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<UserResponse>>> GetUsers()
            => Ok(await service.GetUsersAsync());


        [HttpGet("{id}")]
        public async Task<ActionResult<UserResponse?>> GetUserById(int id)
        {
            var user = await service.GetUserByIdAsync(id);
            return user is null ? NotFound("User not found") : Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult<UserResponse>> CreateUser(CreateUpdateUserRequest user)
        {
            var createdUser = await service.CreateUserAsync(user);
            return CreatedAtAction(nameof(GetUserById), new { id = createdUser.Id }, createdUser);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<UserResponse>> UpdateUser(int id, CreateUpdateUserRequest user)
        {
            var updatedUser = await service.UpdateUserAsync(id, user);
            return updatedUser ? NotFound("User not found") : Ok(updatedUser);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<UserResponse>> DeleteUser(int id)
        {
            var deletedUser = await service.DeleteUserAsync(id);
            return deletedUser ? NotFound("User not found") : Ok(deletedUser);
        }
    }
}
