using System;
using System.Collections.Generic;

namespace BlogSpace.Domain.DTOs
{
    public class PostDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;

        public Guid AuthorId { get; set; } // Added
        public string AuthorName { get; set; } = null!;

        public Guid? CategoryId { get; set; } // Added
        public string? CategoryName { get; set; }

        public List<Guid>? TagIds { get; set; } = new(); // Added
        public List<string>? TagNames { get; set; } = new();

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
