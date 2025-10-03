using System.ComponentModel.DataAnnotations;

namespace BlogSpace.Domain.Entities
{
    public class PostTag
    {
        [Required]
        public Guid PostId { get; set; }
        public virtual Post Post { get; set; } = null!;

        [Required]
        public Guid TagId { get; set; }
        public virtual Tag Tag { get; set; } = null!;
    }
}
