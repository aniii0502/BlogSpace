using System.Security.Claims;
using BlogSpace.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogSpace.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LikesController : ControllerBase
    {
        private readonly ILikeService _likeService;

        public LikesController(ILikeService likeService)
        {
            _likeService = likeService;
        }

        // -------------------------
        // Like a post
        // -------------------------
        [HttpPost("{postId}")]
        [Authorize]
        public async Task<IActionResult> LikePost(Guid postId)
        {
            var userIdClaim = User
                .Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)
                ?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized();

            var like = await _likeService.AddLikeAsync(postId, Guid.Parse(userIdClaim));
            if (like == null)
                return BadRequest(new { message = "Failed to like post" });

            return Ok(new { message = "Post liked successfully" });
        }

        // -------------------------
        // Unlike a post
        // -------------------------
        [HttpDelete("{postId}")]
        [Authorize]
        public async Task<IActionResult> UnlikePost(Guid postId)
        {
            var userIdClaim = User
                .Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)
                ?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized();

            await _likeService.RemoveLikeAsync(postId, Guid.Parse(userIdClaim));
            return Ok(new { message = "Post unliked successfully" });
        }

        // -------------------------
        // Get total likes for a post
        // -------------------------
        [HttpGet("{postId}/count")]
        [AllowAnonymous]
        public async Task<IActionResult> GetLikesCount(Guid postId)
        {
            var count = await _likeService.GetLikesCountAsync(postId);
            return Ok(new { postId, likes = count });
        }
    }
}
