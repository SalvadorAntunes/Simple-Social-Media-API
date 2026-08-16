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
    public class CommentController(ICommentService service) : Controller
    {
        [HttpGet]
        public async Task<ActionResult<List<Comment>>> GetComments()
            => Ok(await service.GetCommentsAsync());

        [HttpGet("{id}")]
        public async Task<ActionResult<Comment?>> GetCommentById(int id)
        {
            var comment = await service.GetCommentByIdAsync(id);
            return comment is null ? NotFound() : Ok(comment);
        }

        [HttpPost]
        public async Task<ActionResult<Comment>> CreateComment(CreateCommentRequest request)
        {
            var comment = await service.CreateCommentAsync(request);
            return CreatedAtAction(nameof(GetCommentById), new { id = comment.Id }, comment);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteComment(int id)
        {
            var result = await service.DeleteCommentAsync(id);
            return result ? Ok(result) : NotFound("Comment not found");
        }
    }
}
