using System.ComponentModel.DataAnnotations;
using SocialMediaAPI.Models;

namespace SocialMediaAPI.Dtos
{
    public class CreateUpdateUserRequest
    {
        //public int Id { get; set; }

        [Required]
        [StringLength(15)]
        public string Username { get; set; } = null!;

        [Required]
        [EmailAddress]
        [StringLength(254)]
        public string Email { get; set; } = null!;

        [Required]
        [StringLength(15)]
        public string UName { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;

        public DateOnly DateOfBirth { get; set; }

        //public DateTime CreatedAt { get; set; }

        //public virtual ICollection<Post> PostsNavigation { get; set; } = new List<Post>();

        //public virtual ICollection<Post> Posts { get; set; } = new List<Post>();
    }
}
