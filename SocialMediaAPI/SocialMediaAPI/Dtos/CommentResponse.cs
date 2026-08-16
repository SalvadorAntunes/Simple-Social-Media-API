using SocialMediaAPI.Models;

namespace SocialMediaAPI.Dtos
{
    public class CommentResponse
    {
        public int PostCommented { get; set; }

        public PostResponse CommentItself { get; set; }

        public virtual Post IdNavigation { get; set; } = null!;

        public virtual Post PostCommentedNavigation { get; set; } = null!;
    }
}
