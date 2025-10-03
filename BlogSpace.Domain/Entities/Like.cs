using System.ComponentModel.DataAnnotations;

namespace BlogSpace.Domain.Entities
{
    public class Like
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid PostId { get; set; }
        public virtual Post Post { get; set; } = null!;

        [Required]
        public Guid UserId { get; set; }
        public virtual User User { get; set; } = null!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
