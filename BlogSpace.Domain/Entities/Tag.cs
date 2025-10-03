using System.ComponentModel.DataAnnotations;

namespace BlogSpace.Domain.Entities
{
    public class Tag
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = null!;

        public virtual ICollection<PostTag> PostTags { get; set; } = new List<PostTag>();
    }
}
