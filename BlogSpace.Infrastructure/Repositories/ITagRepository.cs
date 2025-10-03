using BlogSpace.Domain.Entities;

namespace BlogSpace.Infrastructure.Repositories
{
    public interface ITagRepository
    {
        Task<Tag> GetByIdAsync(Guid tagId);
        Task<Tag> GetByNameAsync(string name);
        Task<IEnumerable<Tag>> GetAllAsync();
        Task AddAsync(Tag tag);
        Task UpdateAsync(Tag tag);
        Task RemoveAsync(Tag tag);
        Task SaveChangesAsync();
    }
}
