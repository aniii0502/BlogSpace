using BlogSpace.Domain.Entities;

namespace BlogSpace.Infrastructure.Repositories
{
    public interface ILikeRepository
    {
        Task<Like> GetByIdAsync(Guid likeId);
        Task<Like> GetByPostAndUserAsync(Guid postId, Guid userId);
        Task<IEnumerable<Like>> GetLikesByPostAsync(Guid postId);
        Task AddAsync(Like like);
        Task RemoveAsync(Like like);
        Task<int> CountLikesAsync(Guid postId);
        Task SaveChangesAsync();
    }
}
