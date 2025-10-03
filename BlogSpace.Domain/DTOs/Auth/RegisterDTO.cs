using System.ComponentModel.DataAnnotations;
using BlogSpace.Domain.Enums;

namespace BlogSpace.Domain.DTOs.Auth
{
    public class RegisterDTO
    {
        [Required]
        [MaxLength(100)]
        public string FullName { get; set; } = null!;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        [MinLength(6)]
        public string Password { get; set; } = null!;
        public UserRole Role { get; set; } = UserRole.User;
    }
}
