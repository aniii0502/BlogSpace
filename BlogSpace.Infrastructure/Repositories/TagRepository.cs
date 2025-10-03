using BlogSpace.Domain.Entities;
using BlogSpace.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BlogSpace.Infrastructure.Repositories
{
    public class TagRepository : ITagRepository
    {
        private readonly AppDbContext _context;

        public TagRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Tag tag)
        {
            await _context.Tags.AddAsync(tag);
        }

        public async Task<IEnumerable<Tag>> GetAllAsync()
        {
            return await _context
                .Tags.Include(t => t.PostTags)
                .ThenInclude(pt => pt.Post)
                .ToListAsync();
        }

        public async Task<Tag> GetByIdAsync(Guid tagId)
        {
            return await _context
                .Tags.Include(t => t.PostTags)
                .ThenInclude(pt => pt.Post)
                .FirstOrDefaultAsync(t => t.Id == tagId);
        }

        public async Task<Tag> GetByNameAsync(string name)
        {
            return await _context
                .Tags.Include(t => t.PostTags)
                .ThenInclude(pt => pt.Post)
                .FirstOrDefaultAsync(t => t.Name == name);
        }

        public async Task RemoveAsync(Tag tag)
        {
            _context.Tags.Remove(tag);
        }

        public async Task UpdateAsync(Tag tag)
        {
            _context.Tags.Update(tag);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
