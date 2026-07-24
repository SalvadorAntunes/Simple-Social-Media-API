using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialMediaAPI.Models;
using SocialMediaAPI.Services;

namespace SocialMediaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IUserService service) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<User>>> GetUsers()
            => Ok(await service.GetUsersAsync());


        [HttpGet("{id}")]
        public async Task<ActionResult<User?>> GetUserById(int id)
        {
            var user = await service.GetUserByIdAsync(id);
            return user is null ? NotFound("User not found") : Ok(user);
        }
    }
}
