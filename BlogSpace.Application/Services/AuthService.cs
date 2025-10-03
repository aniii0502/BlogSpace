using BlogSpace.Application.Interfaces;
using BlogSpace.Common.Helpers;
using BlogSpace.Domain.DTOs;
using BlogSpace.Domain.DTOs.Auth;
using BlogSpace.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace BlogSpace.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        private readonly IConfiguration _configuration;

        public AuthService(
            UserManager<User> userManager,
            RoleManager<IdentityRole<Guid>> roleManager,
            IConfiguration configuration
        )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        // ----------------------------- REGISTER -----------------------------
        public async Task<UserDTO> RegisterAsync(RegisterDTO registerDto)
        {
            // Check if user exists
            var existingUser = await _userManager.FindByEmailAsync(registerDto.Email);
            if (existingUser != null)
                throw new ApplicationException("Email already registered.");

            // Create user with default role
            var user = new User
            {
                UserName = registerDto.Email,
                Email = registerDto.Email,
                FullName = registerDto.FullName,
                Role = Domain.Enums.UserRole.User,
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded)
                throw new ApplicationException(
                    string.Join("; ", result.Errors.Select(e => e.Description))
                );

            // Assign default role in Identity
            string roleName = user.Role.ToString();
            if (!await _roleManager.RoleExistsAsync(roleName))
                await _roleManager.CreateAsync(new IdentityRole<Guid> { Name = roleName });

            await _userManager.AddToRoleAsync(user, roleName);

            return new UserDTO
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role,
            };
        }

        // ----------------------------- LOGIN -----------------------------
        public async Task<string> LoginAsync(LoginDTO loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, loginDto.Password))
                throw new ApplicationException("Invalid email or password.");

            // Read JWT settings safely
            string? secret = _configuration["JwtSettings:SecretKey"];
            string? issuer = _configuration["JwtSettings:Issuer"];
            string? audience = _configuration["JwtSettings:Audience"];
            int expireMinutes = _configuration.GetValue<int>("JwtSettings:ExpirationMinutes");

            if (string.IsNullOrEmpty(secret))
                throw new ApplicationException("JWT SecretKey is missing in configuration.");

            return JwtHelper.GenerateToken(
                user.Id,
                user.Email ?? "",
                user.Role.ToString(),
                secret,
                issuer ?? "",
                audience ?? "",
                expireMinutes
            );
        }

        // ----------------------------- LOGOUT -----------------------------
        public Task LogoutAsync(Guid userId)
        {
            // JWT is stateless, logout handled client-side
            return Task.CompletedTask;
        }
    }
}
