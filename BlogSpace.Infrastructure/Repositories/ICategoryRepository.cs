using BlogSpace.Domain.Entities;

namespace BlogSpace.Infrastructure.Repositories
{
    public interface ICategoryRepository
    {
        Task<Category> GetByIdAsync(Guid categoryId);
        Task<Category> GetByNameAsync(string name);
        Task<IEnumerable<Category>> GetAllAsync();
        Task AddAsync(Category category);
        Task UpdateAsync(Category category);
        Task RemoveAsync(Category category);
        Task SaveChangesAsync();
    }
}
