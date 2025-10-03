using System;

namespace BlogSpace.Domain.DTOs
{
    public class LikeDTO
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid PostId { get; set; }
        public DateTime CreatedAt { get; set; } // Added
    }
}
