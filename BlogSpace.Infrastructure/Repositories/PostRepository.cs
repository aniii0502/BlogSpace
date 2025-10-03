using BlogSpace.Domain.Entities;
using BlogSpace.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BlogSpace.Infrastructure.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly AppDbContext _context;

        public PostRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Post post)
        {
            await _context.Posts.AddAsync(post);
        }

        public async Task<IEnumerable<Post>> GetAllAsync()
        {
            return await _context
                .Posts.Include(p => p.Author)
                .Include(p => p.Comments)
                .Include(p => p.Likes)
                .Include(p => p.Category)
                .Include(p => p.PostTags)
                .ThenInclude(pt => pt.Tag)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task<Post> GetByIdAsync(Guid postId)
        {
            return await _context
                .Posts.Include(p => p.Author)
                .Include(p => p.Comments)
                .Include(p => p.Likes)
                .Include(p => p.Category)
                .Include(p => p.PostTags)
                .ThenInclude(pt => pt.Tag)
                .FirstOrDefaultAsync(p => p.Id == postId);
        }

        public async Task<IEnumerable<Post>> GetByAuthorIdAsync(Guid authorId)
        {
            return await _context
                .Posts.Where(p => p.AuthorId == authorId)
                .Include(p => p.Category)
                .Include(p => p.PostTags)
                .ThenInclude(pt => pt.Tag)
                .Include(p => p.Likes)
                .Include(p => p.Comments)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Post>> GetByCategoryIdAsync(Guid categoryId)
        {
            return await _context
                .Posts.Where(p => p.CategoryId == categoryId)
                .Include(p => p.Author)
                .Include(p => p.PostTags)
                .ThenInclude(pt => pt.Tag)
                .Include(p => p.Likes)
                .Include(p => p.Comments)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Post>> GetByTagIdAsync(Guid tagId)
        {
            return await _context
                .PostTags.Where(pt => pt.TagId == tagId)
                .Select(pt => pt.Post)
                .Include(p => p.Author)
                .Include(p => p.Category)
                .Include(p => p.Likes)
                .Include(p => p.Comments)
                .Include(p => p.PostTags)
                .ThenInclude(pt => pt.Tag)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task RemoveAsync(Post post)
        {
            _context.Posts.Remove(post);
        }

        public async Task UpdateAsync(Post post)
        {
            _context.Posts.Update(post);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
