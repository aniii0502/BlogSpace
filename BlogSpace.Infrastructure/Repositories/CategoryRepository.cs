using BlogSpace.Domain.Entities;
using BlogSpace.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BlogSpace.Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _context;

        public CategoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Category category)
        {
            await _context.Categories.AddAsync(category);
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _context.Categories
                .Include(c => c.Posts)
                .ToListAsync();
        }

        public async Task<Category> GetByIdAsync(Guid categoryId)
        {
            return await _context.Categories
                .Include(c => c.Posts)
                .FirstOrDefaultAsync(c => c.Id == categoryId);
        }

        public async Task<Category> GetByNameAsync(string name)
        {
            return await _context.Categories
                .Include(c => c.Posts)
                .FirstOrDefaultAsync(c => c.Name == name);
        }

        public async Task RemoveAsync(Category category)
        {
            _context.Categories.Remove(category);
        }

        public async Task UpdateAsync(Category category)
        {
            _context.Categories.Update(category);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
