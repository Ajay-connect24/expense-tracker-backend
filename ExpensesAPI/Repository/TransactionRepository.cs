
using ExpensesAPI.Contracts;
using ExpensesAPI.Data;
using ExpensesAPI.Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpensesAPI.Repository
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly ApplicationDbContext _context;

        public TransactionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Transaction> CreateTransactionAsync(Transaction transaction)
        {
            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();
            return transaction;
        }

        public async Task<IEnumerable<Transaction>> GetAllTransactionsAsync(int userId)
        {
            return await _context.Transactions
                .Where(t => t.UserId == userId)
                .Include(t => t.Category) // Include category details if needed
                .ToListAsync();
        }

        public async Task<Transaction> UpdateTransactionAsync(Transaction transaction)
        {
            _context.Transactions.Update(transaction);
            await _context.SaveChangesAsync();
            return transaction;
        }

        public async Task<bool> DeleteTransactionAsync(int transactionId, int userId)
        {
            var transaction = await _context.Transactions
                .FirstOrDefaultAsync(t => t.Id == transactionId && t.UserId == userId);

            if (transaction == null)
            {
                return false; // Transaction not found
            }

            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync();
            return true; // Successfully deleted
        }

        public async Task<IEnumerable<Transaction>> GetWeeklyTransactionsAsync(int userId)
        {
            var startDate = DateTime.Now.AddDays(-7);
            return await _context.Transactions
                .Where(t => t.UserId == userId && t.Date >= startDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Transaction>> GetMonthlyTransactionsAsync(int userId)
        {
            var startDate = DateTime.Now.AddMonths(-1);
            return await _context.Transactions
                .Where(t => t.UserId == userId && t.Date >= startDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Transaction>> GetCustomRangeTransactionsAsync(int userId, DateTime startDate, DateTime endDate)
        {
            return await _context.Transactions
                .Where(t => t.UserId == userId && t.Date >= startDate && t.Date <= endDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Transaction>> GetDailyTransactionsAsync(int userId)
        {
            var startDate = DateTime.Today;
            var endDate = startDate.AddDays(1).AddTicks(-1); // End of the day

            return await _context.Transactions
                .Where(t => t.UserId == userId && t.Date >= startDate && t.Date <= endDate)
                .ToListAsync();
        }
    }
}