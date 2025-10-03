using System;

namespace BlogSpace.Domain.DTOs
{
    public class CategoryDTO
    {
        public Guid Id { get; set; } // Unique identifier for the category
        public string Name { get; set; } = string.Empty; // Category name
        public string? Description { get; set; } // Optional description
    }
}
