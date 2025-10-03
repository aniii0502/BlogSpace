using System.Security.Claims;
using BlogSpace.Application.Interfaces;
using BlogSpace.Domain.DTOs.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogSpace.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        // ======================
        // REGISTER
        // ======================
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _authService.RegisterAsync(registerDto);

            return Ok(
                new
                {
                    message = "User registered successfully",
                    userId = user.Id,
                    fullName = user.FullName,
                    email = user.Email,
                    role = user.Role.ToString(),
                }
            );
        }

        // ======================
        // LOGIN
        // ======================
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var token = await _authService.LoginAsync(loginDto);
            return Ok(new { token });
        }

        // ======================
        // LOGOUT
        // ======================
        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            // Get the user ID from JWT claim
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized();

            await _authService.LogoutAsync(Guid.Parse(userIdClaim));

            return Ok(new { message = "Logged out successfully" });
        }
    }
}
