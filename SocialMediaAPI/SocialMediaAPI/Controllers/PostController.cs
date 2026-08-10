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
    public class PostController (IPostService service) : Controller
    {

        [HttpGet]
        public async Task<ActionResult<List<PostResponse>>> GetPosts()
            => Ok(await service.GetPostsAsync());

        [HttpGet("{id}")]
        public async Task<ActionResult<PostResponse?>> GetPostById(int id)
        {
            var post = await service.GetPostByIdAsync(id);
            return post is null ? NotFound("Post not found") : Ok(post);
        }

        [HttpPost]
        public async Task<ActionResult<Post>> CreatePost(CreatePostRequest request)
        {
            var post = await service.CreatePostAsync(request);
            return CreatedAtAction(nameof(GetPostById), new { id = post.Id }, post);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeletePost(int id)
        {
            var result = await service.DeletePostAsync(id);
            return result ? Ok(result) : NotFound("Post not found");
        }
    }
}
