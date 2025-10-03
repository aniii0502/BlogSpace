using BlogSpace.Domain.Entities;
using BlogSpace.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BlogSpace.Infrastructure.Repositories
{
    public class LikeRepository : ILikeRepository
    {
        private readonly AppDbContext _context;

        public LikeRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Like like)
        {
            await _context.Likes.AddAsync(like);
        }

        public async Task<int> CountLikesAsync(Guid postId)
        {
            return await _context.Likes.CountAsync(l => l.PostId == postId);
        }

        public async Task<Like> GetByIdAsync(Guid likeId)
        {
            return await _context.Likes.FirstOrDefaultAsync(l => l.Id == likeId);
        }

        public async Task<Like> GetByPostAndUserAsync(Guid postId, Guid userId)
        {
            return await _context.Likes.FirstOrDefaultAsync(l =>
                l.PostId == postId && l.UserId == userId
            );
        }

        public async Task<IEnumerable<Like>> GetLikesByPostAsync(Guid postId)
        {
            return await _context.Likes.Where(l => l.PostId == postId).ToListAsync();
        }

        public async Task RemoveAsync(Like like)
        {
            _context.Likes.Remove(like);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
