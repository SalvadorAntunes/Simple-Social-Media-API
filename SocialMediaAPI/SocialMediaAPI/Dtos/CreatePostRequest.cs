using SocialMediaAPI.Models;

namespace SocialMediaAPI.Dtos
{
    public class CreatePostRequest
    {
        //public int Id { get; set; }

        public int UserId { get; set; }

        //public DateTime PostDate { get; set; }

        public string Text { get; set; } = null!;

        //public virtual Comment? CommentIdNavigation { get; set; }

        //public virtual ICollection<Comment> CommentPostCommentedNavigations { get; set; } = new List<Comment>();

        //public virtual User User { get; set; } = null!;

        //public virtual ICollection<User> Users { get; set; } = new List<User>();
    }
}
