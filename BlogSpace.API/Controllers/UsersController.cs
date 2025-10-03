using System.Security.Claims;
using BlogSpace.Application.Interfaces;
using BlogSpace.Domain.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogSpace.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // All endpoints require authentication
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        // -------------------------
        // Get current logged-in user profile
        // -------------------------
        [HttpGet("me")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var userIdClaim = User
                .Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)
                ?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized();

            var user = await _userService.GetUserByIdAsync(Guid.Parse(userIdClaim));
            if (user == null)
                return NotFound(new { message = "User not found" });

            return Ok(user);
        }

        // -------------------------
        // Update current user profile
        // -------------------------
        [HttpPut("me")]
        public async Task<IActionResult> UpdateProfile([FromBody] UserDTO userDto)
        {
            var userIdClaim = User
                .Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)
                ?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized();

            var updatedUser = await _userService.UpdateUserAsync(Guid.Parse(userIdClaim), userDto);
            if (updatedUser == null)
                return BadRequest(new { message = "Failed to update user" });

            return Ok(updatedUser);
        }

        // -------------------------
        // Get user by ID (Admin only)
        // -------------------------
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
                return NotFound(new { message = "User not found" });

            return Ok(user);
        }

        // -------------------------
        // List all users (Admin only)
        // -------------------------
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }
    }
}
