using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PersonalFinanceTracker.Dtos.CategoryDtos;
using PersonalFinanceTracker.Entities;
using PersonalFinanceTracker.Mappers;
using PersonalFinanceTracker.Models;

namespace PersonalFinanceTracker.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Category>> GetCategoryById([FromRoute] int id) {
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.CategoryId == id);

            if (category == null) return NotFound();

            return Ok(category);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryDto categoryDto) {

            if (!ModelState.IsValid) return BadRequest();

            var categoryModel = categoryDto.FromCreateDtoToCategory();
            await _context.Categories.AddAsync(categoryModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCategoryById), new { id = categoryModel.CategoryId }, categoryModel);

        }

        [HttpPut("{id:int}")]
         public async Task<IActionResult> UpdateCategory([FromRoute] int id ,[FromBody] UpdateCategoryDto categoryDto)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.CategoryId == id);
            if (category == null) return NotFound();

            category.Name = categoryDto.Name;
            category.Description = categoryDto.Description;

            await _context.SaveChangesAsync();

            return Ok(category);
        }

        [HttpDelete("{id:int}")]
        public async Task<Category> DeleteCategory([FromRoute] int id) {
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.CategoryId == id);
            if (category == null) return null;
            _context.Categories.Remove(category);
            return category;
        }
        
    }
}
