using Microsoft.AspNetCore.Mvc;
using Repository.Models;
using Service;
using Repository;
using Microsoft.AspNetCore.Authorization;

namespace Assignment2_PRN232.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly CategoryService _service;

        public CategoryController()
        {
            _service = new CategoryService();
        }
        [Authorize(Roles = "1")]
        [HttpGet("GetAllCategories")]
        public async Task<IActionResult> GetAll()
        {
            var data = await _service.GetAllCategories();
            return Ok(data);
        }
        [Authorize(Roles = "1")]
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(short id)
        {
            var category = await _service.GetCategoryById(id);
            if (category == null) return NotFound();
            return Ok(category);
        }
        [Authorize(Roles = "1")]
        [HttpPost("CreateCategory")]
        public async Task<IActionResult> Create([FromBody] Category category)
        {
            var success = await _service.CreateCategory(
                category.CategoryName,
                category.CategoryDesciption,
                category.ParentCategoryId,
                category.IsActive
            );
            if (!success) return BadRequest();
            return Ok("Category created.");
        }
        [Authorize(Roles = "1")]
        [HttpPut("UpdateCategory/{id}")]
        public async Task<IActionResult> Update(short id, [FromBody] Category category)
        {
            var success = await _service.UpdateCategory(
                id,
                category.CategoryName,
                category.CategoryDesciption,
                category.ParentCategoryId,
                category.IsActive
            );
            if (!success) return BadRequest("Update failed.");
            return Ok("Category updated.");
        }
        [Authorize(Roles = "1")]
        [HttpDelete("DeleteCategory/{id}")]
        public async Task<IActionResult> Delete(short id)
        {
            var success = await _service.DeleteCategory(id);
            if (!success) return BadRequest("Cannot delete this category.");
            return Ok("Category deleted.");
        }
    }
}
