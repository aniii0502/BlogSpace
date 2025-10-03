using System.ComponentModel.DataAnnotations;

namespace BlogSpace.Domain.Entities
{
    public class Comment
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public string Content { get; set; } = null!;

        [Required]
        public Guid PostId { get; set; }
        public virtual Post Post { get; set; } = null!;

        [Required]
        public Guid AuthorId { get; set; }
        public virtual User Author { get; set; } = null!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
