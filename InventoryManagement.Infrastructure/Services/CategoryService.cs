using InventoryManagement.Common.Dtos;
using InventoryManagement.Core.Entities;
using InventoryManagement.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Infrastructure.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<List<CategoryDto>> GetAllCategories()
        {
            var categories = await _categoryRepository.GetAllAsync();
            return categories.Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description // এখন এটি কাজ করবে
            }).ToList();
        }

        public async Task AddCategory(CategoryDto categoryDto)
        {
            var category = new Category
            {
                Name = categoryDto.Name,
                Description = categoryDto.Description, // যোগ করা হয়েছে
                CreatedAt = DateTime.Now
            };
            await _categoryRepository.AddAsync(category);
        }

        public async Task UpdateCategory(int id, CategoryDto categoryDto)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category != null)
            {
                category.Name = categoryDto.Name;
                category.Description = categoryDto.Description; // যোগ করা হয়েছে
                category.CreatedAt = DateTime.Now; // অপশনাল
                await _categoryRepository.UpdateAsync(category);
            }
        }

        public async Task DeleteCategory(int id)
        {
            await _categoryRepository.DeleteAsync(id);
        }
    }
}