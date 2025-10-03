using System;

namespace BlogSpace.Domain.DTOs
{
    public class CommentDTO
    {
        public Guid Id { get; set; }
        public string Content { get; set; } = null!;

        public Guid PostId { get; set; }          // Added
        public Guid AuthorId { get; set; }        // Added
        public string AuthorName { get; set; } = null!;

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
