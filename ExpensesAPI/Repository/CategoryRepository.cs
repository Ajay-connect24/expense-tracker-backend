
using ExpensesAPI.Contracts;
using ExpensesAPI.Data;
using ExpensesAPI.Entities.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpensesAPI.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Category> CreateCategoryAsync(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync(int userId)
        {
            return await _context.Categories
                .Where(c => c.UserId == userId)
                .ToListAsync();
        }

        public async Task<bool> DeleteCategoryAsync(int categoryId, int userId)
        {
            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == categoryId && c.UserId == userId);

            if (category == null)
            {
                return false; // Category not found
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return true; // Successfully deleted
        }
    }
}