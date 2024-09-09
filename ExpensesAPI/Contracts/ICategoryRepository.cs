// Location: ExpensesAPI.Contracts/ICategoryRepository.cs
using ExpensesAPI.Entities.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExpensesAPI.Contracts
{
    public interface ICategoryRepository
    {
        Task<Category> CreateCategoryAsync(Category category);
        Task<IEnumerable<Category>> GetAllCategoriesAsync(int userId);
        Task<bool> DeleteCategoryAsync(int categoryId, int userId);
    }
}