using BlogSpace.Application.Interfaces;
using BlogSpace.Domain.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogSpace.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagsController : ControllerBase
    {
        private readonly ITagService _tagService;

        public TagsController(ITagService tagService)
        {
            _tagService = tagService;
        }

        // -------------------------
        // Get all tags
        // -------------------------
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var tags = await _tagService.GetAllTagsAsync();
            return Ok(tags);
        }

        // -------------------------
        // Get tag by ID
        // -------------------------
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(Guid id)
        {
            var tag = await _tagService.GetTagByIdAsync(id);
            if (tag == null)
                return NotFound(new { message = "Tag not found" });

            return Ok(tag);
        }

        // -------------------------
        // Create new tag (Admin only)
        // -------------------------
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] TagDTO tagDto)
        {
            var createdTag = await _tagService.CreateTagAsync(tagDto);
            return Ok(createdTag);
        }

        // -------------------------
        // Update tag (Admin only)
        // -------------------------
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(Guid id, [FromBody] TagDTO tagDto)
        {
            var updatedTag = await _tagService.UpdateTagAsync(id, tagDto);
            if (updatedTag == null)
                return BadRequest(new { message = "Failed to update tag" });

            return Ok(updatedTag);
        }

        // -------------------------
        // Delete tag (Admin only)
        // -------------------------
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _tagService.DeleteTagAsync(id);
            return Ok(new { message = "Tag deleted successfully" });
        }
    }
}
