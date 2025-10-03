using System.ComponentModel.DataAnnotations;
using BlogSpace.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace BlogSpace.Domain.Entities
{
    public class User : IdentityUser<Guid>
    {
        [Required]
        [MaxLength(100)]
        public string FullName { get; set; } = null!;

        public UserRole Role { get; set; } = UserRole.User;

        // Navigation properties
        public virtual ICollection<Post> Posts { get; set; } = new List<Post>();
        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public virtual ICollection<Like> Likes { get; set; } = new List<Like>();
    }
}
