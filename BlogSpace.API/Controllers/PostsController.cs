using System.Security.Claims;
using BlogSpace.Application.Interfaces;
using BlogSpace.Domain.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogSpace.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly IPostService _postService;

        public PostsController(IPostService postService)
        {
            _postService = postService;
        }

        // -------------------------
        // Get all posts
        // -------------------------
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var posts = await _postService.GetAllPostsAsync();
            return Ok(posts);
        }

        // -------------------------
        // Get post by ID
        // -------------------------
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(Guid id)
        {
            var post = await _postService.GetPostByIdAsync(id);
            if (post == null)
                return NotFound(new { message = "Post not found" });

            return Ok(post);
        }

        // -------------------------
        // Get posts by category
        // -------------------------
        [HttpGet("category/{categoryId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByCategory(Guid categoryId)
        {
            var posts = await _postService.GetPostsByCategoryAsync(categoryId);
            return Ok(posts);
        }

        // -------------------------
        // Get posts by tag
        // -------------------------
        [HttpGet("tag/{tagId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByTag(Guid tagId)
        {
            var posts = await _postService.GetPostsByTagAsync(tagId);
            return Ok(posts);
        }

        // -------------------------
        // Create new post
        // -------------------------
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] PostDTO postDto)
        {
            var userIdClaim = User
                .Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)
                ?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized();

            var createdPost = await _postService.CreatePostAsync(postDto, Guid.Parse(userIdClaim));
            return Ok(createdPost);
        }

        // -------------------------
        // Update post
        // -------------------------
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Update(Guid id, [FromBody] PostDTO postDto)
        {
            var userIdClaim = User
                .Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)
                ?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized();

            var updatedPost = await _postService.UpdatePostAsync(
                id,
                postDto,
                Guid.Parse(userIdClaim)
            );
            if (updatedPost == null)
                return BadRequest(new { message = "Failed to update post" });

            return Ok(updatedPost);
        }

        // -------------------------
        // Delete post
        // -------------------------
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(Guid id)
        {
            var userIdClaim = User
                .Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)
                ?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized();

            await _postService.DeletePostAsync(id, Guid.Parse(userIdClaim));
            return Ok(new { message = "Post deleted successfully" });
        }
    }
}
