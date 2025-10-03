using BlogSpace.Application.Interfaces;
using BlogSpace.Domain.DTOs;
using BlogSpace.Domain.Entities;
using BlogSpace.Common.Exceptions;
using BlogSpace.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BlogSpace.Application.Services
{
    public class TagService : ITagService
    {
        private readonly AppDbContext _context;

        public TagService(AppDbContext context)
        {
            _context = context;
        }

        // -----------------------------
        // CREATE TAG
        // -----------------------------
        public async Task<TagDTO> CreateTagAsync(TagDTO tagDto)
        {
            // Check if tag with same name exists
            if (await _context.Tags.AnyAsync(t => t.Name == tagDto.Name))
                throw new ApplicationException("Tag already exists.");

            var tag = new Tag
            {
                Id = Guid.NewGuid(),
                Name = tagDto.Name
            };

            _context.Tags.Add(tag);
            await _context.SaveChangesAsync();

            return new TagDTO
            {
                Id = tag.Id,
                Name = tag.Name
            };
        }

        // -----------------------------
        // UPDATE TAG
        // -----------------------------
        public async Task<TagDTO> UpdateTagAsync(Guid tagId, TagDTO tagDto)
        {
            var tag = await _context.Tags.FindAsync(tagId);
            if (tag == null) throw new NotFoundException("Tag not found.");

            tag.Name = tagDto.Name;
            await _context.SaveChangesAsync();

            return new TagDTO
            {
                Id = tag.Id,
                Name = tag.Name
            };
        }

        // -----------------------------
        // DELETE TAG
        // -----------------------------
        public async Task DeleteTagAsync(Guid tagId)
        {
            var tag = await _context.Tags.FindAsync(tagId);
            if (tag == null) throw new NotFoundException("Tag not found.");

            _context.Tags.Remove(tag);
            await _context.SaveChangesAsync();
        }

        // -----------------------------
        // GET ALL TAGS
        // -----------------------------
        public async Task<IEnumerable<TagDTO>> GetAllTagsAsync()
        {
            var tags = await _context.Tags.ToListAsync();
            return tags.Select(t => new TagDTO
            {
                Id = t.Id,
                Name = t.Name
            });
        }

        // -----------------------------
        // GET TAG BY ID
        // -----------------------------
        public async Task<TagDTO> GetTagByIdAsync(Guid tagId)
        {
            var tag = await _context.Tags.FindAsync(tagId);
            if (tag == null) throw new NotFoundException("Tag not found.");

            return new TagDTO
            {
                Id = tag.Id,
                Name = tag.Name
            };
        }
    }
}
