using transaction_tracker.Entities.DataTransferObjects;
using transaction_tracker.Entities.Models;

namespace transaction_tracker.Contracts
{
    public interface IExpenseRepository
    {
        Task<List<TransactionDto>> GetLast10Transactions(Guid userId);
        Task<decimal> GetTotalMonthlyExpense(Guid userId, int month, int year);
        Task<Dictionary<string, decimal>> GetMonthlyExpenses(Guid userId);
        Task<Dictionary<int, decimal>> GetDailyExpenses(Guid userId, int month, int year);
        Task<Dictionary<string, decimal>> GetExpenseCategory(Guid userId, int month, int year);
        Task CreateExpense(Expense expense);
        Task<Expense> GetExpenseById(Guid userId, int id);
        Task UpdateExpense(Expense expense);
        Task DeleteExpense(int id);
    }
}