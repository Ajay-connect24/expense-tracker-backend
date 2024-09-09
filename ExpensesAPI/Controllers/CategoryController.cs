
using ExpensesAPI.Contracts;
using ExpensesAPI.Entities.DataTransferObjects;
using ExpensesAPI.Entities.Models;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ExpensesAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CategoryController(ICategoryRepository categoryRepository, IHttpContextAccessor httpContextAccessor)
        {
            _categoryRepository = categoryRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryDto categoryDto)
        {
            var userId = GetUserIdFromClaims();
            if (userId == null)
            {
                return Unauthorized();
            }

            var category = categoryDto.Adapt<Category>();
            category.UserId = userId.Value; // Set the UserId from claims

            var createdCategory = await _categoryRepository.CreateCategoryAsync(category);
            return CreatedAtAction(nameof(CreateCategory), new { id = createdCategory.Id }, createdCategory.Adapt<CategoryDto>());
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var userId = GetUserIdFromClaims();
            if (userId == null)
            {
                return Unauthorized();
            }

            var categories = await _categoryRepository.GetAllCategoriesAsync(userId.Value);
            return Ok(categories.Adapt<IEnumerable<CategoryDto>>());
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var userId = GetUserIdFromClaims();
            if (userId == null)
            {
                return Unauthorized();
            }

            var result = await _categoryRepository.DeleteCategoryAsync(id, userId.Value);
            if (!result)
            {
                return NotFound(); // Category not found
            }

            return NoContent(); // Successfully deleted
        }

        private int? GetUserIdFromClaims()
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);
            return userIdClaim != null ? int.Parse(userIdClaim.Value) : (int?)null;
        }
    }
}