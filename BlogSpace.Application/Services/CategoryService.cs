using BlogSpace.Application.Interfaces;
using BlogSpace.Common.Exceptions;
using BlogSpace.Domain.DTOs;
using BlogSpace.Domain.Entities;
using BlogSpace.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BlogSpace.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly AppDbContext _context;

        public CategoryService(AppDbContext context)
        {
            _context = context;
        }

        // -----------------------------
        // CREATE CATEGORY
        // -----------------------------
        public async Task<CategoryDTO> CreateCategoryAsync(CategoryDTO categoryDto)
        {
            if (await _context.Categories.AnyAsync(c => c.Name == categoryDto.Name))
                throw new ApplicationException("Category already exists.");

            var category = new Category { Id = Guid.NewGuid(), Name = categoryDto.Name };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return new CategoryDTO { Id = category.Id, Name = category.Name };
        }

        // -----------------------------
        // UPDATE CATEGORY
        // -----------------------------
        public async Task<CategoryDTO> UpdateCategoryAsync(Guid categoryId, CategoryDTO categoryDto)
        {
            var category = await _context.Categories.FindAsync(categoryId);
            if (category == null)
                throw new NotFoundException("Category not found.");

            category.Name = categoryDto.Name;
            await _context.SaveChangesAsync();

            return new CategoryDTO { Id = category.Id, Name = category.Name };
        }

        // -----------------------------
        // DELETE CATEGORY
        // -----------------------------
        public async Task DeleteCategoryAsync(Guid categoryId)
        {
            var category = await _context.Categories.FindAsync(categoryId);
            if (category == null)
                throw new NotFoundException("Category not found.");

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }

        // -----------------------------
        // GET ALL CATEGORIES
        // -----------------------------
        public async Task<IEnumerable<CategoryDTO>> GetAllCategoriesAsync()
        {
            var categories = await _context.Categories.ToListAsync();
            return categories.Select(c => new CategoryDTO { Id = c.Id, Name = c.Name });
        }

        // -----------------------------
        // GET CATEGORY BY ID
        // -----------------------------
        public async Task<CategoryDTO> GetCategoryByIdAsync(Guid categoryId)
        {
            var category = await _context.Categories.FindAsync(categoryId);
            if (category == null)
                throw new NotFoundException("Category not found.");

            return new CategoryDTO { Id = category.Id, Name = category.Name };
        }
    }
}
