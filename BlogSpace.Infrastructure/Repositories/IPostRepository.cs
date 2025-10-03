using BlogSpace.Domain.Entities;

namespace BlogSpace.Infrastructure.Repositories
{
    public interface IPostRepository
    {
        Task<Post> GetByIdAsync(Guid postId);
        Task<IEnumerable<Post>> GetAllAsync();
        Task<IEnumerable<Post>> GetByAuthorIdAsync(Guid authorId);
        Task<IEnumerable<Post>> GetByCategoryIdAsync(Guid categoryId);
        Task<IEnumerable<Post>> GetByTagIdAsync(Guid tagId);
        Task AddAsync(Post post);
        Task UpdateAsync(Post post);
        Task RemoveAsync(Post post);
        Task SaveChangesAsync();
    }
}
