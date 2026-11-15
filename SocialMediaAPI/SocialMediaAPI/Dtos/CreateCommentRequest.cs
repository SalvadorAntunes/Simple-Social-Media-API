using System.ComponentModel.DataAnnotations;

namespace SocialMediaAPI.Dtos
{
    public class CreateCommentRequest
    {
        [Required]
        public CreatePostRequest PostInfo { get; set; } = null!;

        public int PostCommented { get; set; }

    }
}
