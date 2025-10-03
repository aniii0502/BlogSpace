using System;

namespace BlogSpace.Domain.DTOs
{
    public class TagDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
    }
}
