using BlogSpace.Domain.DTOs;

namespace BlogSpace.Application.Interfaces
{
    public interface ICategoryService
    {
        Task<CategoryDTO> CreateCategoryAsync(CategoryDTO categoryDto);
        Task<IEnumerable<CategoryDTO>> GetAllCategoriesAsync();
        Task<CategoryDTO> GetCategoryByIdAsync(Guid categoryId);
        Task<CategoryDTO> UpdateCategoryAsync(Guid categoryId, CategoryDTO categoryDto);
        Task DeleteCategoryAsync(Guid categoryId);
    }
}
