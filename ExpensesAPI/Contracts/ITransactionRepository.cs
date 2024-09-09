
using ExpensesAPI.Entities.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExpensesAPI.Contracts
{
    public interface ITransactionRepository
    {
        Task<Transaction> CreateTransactionAsync(Transaction transaction);
        Task<IEnumerable<Transaction>> GetAllTransactionsAsync(int userId);
        Task<Transaction> UpdateTransactionAsync(Transaction transaction);
        Task<bool> DeleteTransactionAsync(int transactionId, int userId);
        Task<IEnumerable<Transaction>> GetDailyTransactionsAsync(int userId);
        Task<IEnumerable<Transaction>> GetWeeklyTransactionsAsync(int userId);
        Task<IEnumerable<Transaction>> GetMonthlyTransactionsAsync(int userId);
        Task<IEnumerable<Transaction>> GetCustomRangeTransactionsAsync(int userId, DateTime startDate, DateTime endDate);
    }
}