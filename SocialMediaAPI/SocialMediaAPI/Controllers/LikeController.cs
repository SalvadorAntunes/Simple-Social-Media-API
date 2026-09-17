using Microsoft.AspNetCore.Mvc;
using SocialMediaAPI.Data;
using SocialMediaAPI.Dtos;
using SocialMediaAPI.Models;
using SocialMediaAPI.Services;

namespace SocialMediaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LikeController(ILikeService service) : Controller
    {
        
        [HttpPost]
        public async Task<ActionResult<bool>> AddPost(Like request)
            => Ok(await service.AddLikeAsync(request));
        

        [HttpDelete]
        public async Task<ActionResult<bool>> RemovePost(Like request)
            => Ok(await service.RemoveLikeAsync(request));
    }
}
