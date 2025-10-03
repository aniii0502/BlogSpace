using BlogSpace.Domain.DTOs;
using BlogSpace.Domain.DTOs.Auth;

namespace BlogSpace.Application.Interfaces
{
    public interface IAuthService
    {
        Task<UserDTO> RegisterAsync(RegisterDTO registerDto);
        Task<string> LoginAsync(LoginDTO loginDto);
        Task LogoutAsync(Guid userId);
    }
}
