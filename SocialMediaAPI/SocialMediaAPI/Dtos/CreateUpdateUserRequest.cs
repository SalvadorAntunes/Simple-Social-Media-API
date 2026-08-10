using SocialMediaAPI.Models;

namespace SocialMediaAPI.Dtos
{
    public class CreateUpdateUserRequest
    {
        //public int Id { get; set; }

        public string Username { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string UName { get; set; } = null!;

        public string Password { get; set; } = null!;

        public DateOnly DateOfBirth { get; set; }

        //public DateTime CreatedAt { get; set; }

        //public virtual ICollection<Post> PostsNavigation { get; set; } = new List<Post>();

        //public virtual ICollection<Post> Posts { get; set; } = new List<Post>();
    }
}
