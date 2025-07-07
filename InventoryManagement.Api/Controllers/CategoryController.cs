using InventoryManagement.Common.Dtos;
using InventoryManagement.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace InventoryManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _categoryService.GetAllCategories();
            return Ok(categories);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CategoryDto categoryDto)
        {
            await _categoryService.AddCategory(categoryDto);
            var categories = await _categoryService.GetAllCategories(); // সমস্ত ক্যাটাগরি লোড করুন
            var addedCategory = categories.Last(); // সর্বশেষ যোগ করা ক্যাটাগরি
            return Ok(addedCategory); // নতুন ক্যাটাগরি ফেরত দিন
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CategoryDto categoryDto)
        {
            await _categoryService.UpdateCategory(id, categoryDto);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _categoryService.DeleteCategory(id);
            return Ok();
        }
    }
}