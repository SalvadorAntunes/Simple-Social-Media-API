using SocialMediaAPI.Models;

namespace SocialMediaAPI.Dtos
{
    public class CommentResponse
    {
        public int PostCommented { get; set; }

        public required PostResponse CommentItself { get; set;}

    }
}
