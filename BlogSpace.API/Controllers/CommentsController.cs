using System.Security.Claims;
using BlogSpace.Application.Interfaces;
using BlogSpace.Domain.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogSpace.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentsController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        // -------------------------
        // Get comments for a post
        // -------------------------
        [HttpGet("post/{postId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCommentsByPost(Guid postId)
        {
            var comments = await _commentService.GetCommentsByPostAsync(postId);
            return Ok(comments);
        }

        // -------------------------
        // Add a comment to a post
        // -------------------------
        [HttpPost("post/{postId}")]
        [Authorize]
        public async Task<IActionResult> AddComment(Guid postId, [FromBody] CommentDTO commentDto)
        {
            var userIdClaim = User
                .Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)
                ?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized();

            var createdComment = await _commentService.AddCommentAsync(
                postId,
                Guid.Parse(userIdClaim),
                commentDto
            );

            return Ok(createdComment);
        }

        // -------------------------
        // Update a comment
        // -------------------------
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateComment(Guid id, [FromBody] CommentDTO commentDto)
        {
            var userIdClaim = User
                .Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)
                ?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized();

            var updatedComment = await _commentService.UpdateCommentAsync(
                id,
                Guid.Parse(userIdClaim),
                commentDto
            );

            if (updatedComment == null)
                return BadRequest(new { message = "Failed to update comment" });

            return Ok(updatedComment);
        }

        // -------------------------
        // Delete a comment
        // -------------------------
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteComment(Guid id)
        {
            var userIdClaim = User
                .Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)
                ?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized();

            await _commentService.DeleteCommentAsync(id, Guid.Parse(userIdClaim));
            return Ok(new { message = "Comment deleted successfully" });
        }
    }
}
