using BlogSpace.Application.Interfaces;
using BlogSpace.Domain.DTOs;
using BlogSpace.Domain.Entities;
using BlogSpace.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace BlogSpace.Application.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;

        public UserService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        // -----------------------------
        // GET USER BY ID
        // -----------------------------
        public async Task<UserDTO> GetUserByIdAsync(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                throw new ApplicationException("User not found.");

            return new UserDTO
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role,
            };
        }

        // -----------------------------
        // GET ALL USERS
        // -----------------------------
        public async Task<IEnumerable<UserDTO>> GetAllUsersAsync()
        {
            var users = _userManager.Users.ToList();

            return users.Select(u => new UserDTO
            {
                Id = u.Id,
                FullName = u.FullName,
                Email = u.Email,
                Role = u.Role,
            });
        }

        // -----------------------------
        // UPDATE USER
        // -----------------------------
        public async Task<UserDTO> UpdateUserAsync(Guid userId, UserDTO userDto)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                throw new ApplicationException("User not found.");

            user.FullName = userDto.FullName;
            user.Email = userDto.Email;
            user.UserName = userDto.Email;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                var errors = string.Join("; ", result.Errors.Select(e => e.Description));
                throw new ApplicationException($"Update failed: {errors}");
            }

            return new UserDTO
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role,
            };
        }

        // -----------------------------
        // DELETE USER
        // -----------------------------
        public async Task DeleteUserAsync(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                throw new ApplicationException("User not found.");

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                var errors = string.Join("; ", result.Errors.Select(e => e.Description));
                throw new ApplicationException($"Delete failed: {errors}");
            }
        }

        // -----------------------------
        // ASSIGN ROLE
        // -----------------------------
        public async Task AssignRoleAsync(Guid userId, UserRole role)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                throw new ApplicationException("User not found.");

            // Remove current roles
            var currentRoles = await _userManager.GetRolesAsync(user);
            var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
            if (!removeResult.Succeeded)
            {
                var errors = string.Join("; ", removeResult.Errors.Select(e => e.Description));
                throw new ApplicationException($"Role removal failed: {errors}");
            }

            // Add new role
            var addResult = await _userManager.AddToRoleAsync(user, role.ToString());
            if (!addResult.Succeeded)
            {
                var errors = string.Join("; ", addResult.Errors.Select(e => e.Description));
                throw new ApplicationException($"Role assignment failed: {errors}");
            }

            // Update role property in user entity
            user.Role = role;
            await _userManager.UpdateAsync(user);
        }
    }
}
