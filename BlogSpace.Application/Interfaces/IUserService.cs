using BlogSpace.Domain.DTOs;
using BlogSpace.Domain.Enums;

namespace BlogSpace.Application.Interfaces
{
    public interface IUserService
    {
        Task<UserDTO> GetUserByIdAsync(Guid userId);
        Task<IEnumerable<UserDTO>> GetAllUsersAsync();
        Task<UserDTO> UpdateUserAsync(Guid userId, UserDTO userDto);
        Task DeleteUserAsync(Guid userId);
        Task AssignRoleAsync(Guid userId, UserRole role);
    }
}
