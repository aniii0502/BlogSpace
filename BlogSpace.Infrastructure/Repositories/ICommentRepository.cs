using BlogSpace.Domain.Entities;

namespace BlogSpace.Infrastructure.Repositories
{
    public interface ICommentRepository
    {
        Task<Comment> GetByIdAsync(Guid commentId);
        Task<IEnumerable<Comment>> GetByPostIdAsync(Guid postId);
        Task AddAsync(Comment comment);
        Task RemoveAsync(Comment comment);
        Task SaveChangesAsync();
    }
}
