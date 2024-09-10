using Microsoft.EntityFrameworkCore;
using Mapster;
using transaction_tracker.Contracts;
using transaction_tracker.Data;
using transaction_tracker.Entities.DataTransferObjects;
using transaction_tracker.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace transaction_tracker.Repository
{
    public class ExpenseRepository : IExpenseRepository
    {
        private readonly TransactionDbContext _dbContext;

        public ExpenseRepository(TransactionDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<TransactionDto>> GetLast10Transactions(Guid userId)
        {
            var expenses = await _dbContext.Expenses
                .Where(e => e.UserId == userId)
                .OrderByDescending(e => e.Date)
                .Take(10)
                .ToListAsync();

            return expenses.Adapt<List<TransactionDto>>();
        }

        public async Task<decimal> GetTotalMonthlyExpense(Guid userId, int month, int year)
        {
            var total = await _dbContext.Expenses
                .Where(e => e.UserId == userId && e.Date.Month == month && e.Date.Year == year)
                .SumAsync(e => e.Amount);

            return total;
        }

        public async Task<Dictionary<string, decimal>> GetMonthlyExpenses(Guid userId)
        {
            var monthlyExpenses = await _dbContext.Expenses
                .Where(e => e.UserId == userId)
                .GroupBy(e => new { e.Date.Month, e.Date.Year })
                .Select(g => new
                {
                    MonthYear = $"{g.Key.Month}/{g.Key.Year}",
                    TotalAmount = g.Sum(e => e.Amount)
                })
                .ToDictionaryAsync(g => g.MonthYear, g => g.TotalAmount);

            return monthlyExpenses;
        }

        public async Task<Dictionary<int, decimal>> GetDailyExpenses(Guid userId, int month, int year)
        {
            var dailyExpenses = await _dbContext.Expenses
                .Where(e => e.UserId == userId && e.Date.Month == month && e.Date.Year == year)
                .GroupBy(e => e.Date.Day)
                .Select(g => new
                {
                    Day = g.Key,
                    TotalAmount = g.Sum(e => e.Amount)
                })
                .ToDictionaryAsync(g => g.Day, g => g.TotalAmount);

            return dailyExpenses;
        }

        public async Task<Dictionary<string, decimal>> GetExpenseCategory(Guid userId, int month, int year)
        {
            var categoryExpenses = await _dbContext.Expenses
                .Where(e => e.UserId == userId && e.Date.Month == month && e.Date.Year == year)
                .GroupBy(e => e.Category)
                .Select(g => new
                {
                    Category = g.Key,
                    TotalAmount = g.Sum(e => e.Amount)
                })
                .ToDictionaryAsync(g => g.Category, g => g.TotalAmount);

            return categoryExpenses;
        }

        public async Task CreateExpense(Expense expense)
        {
            await _dbContext.Expenses.AddAsync(expense);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Expense> GetExpenseById(Guid userId, int id)
        {
            return await _dbContext.Expenses
                .FirstOrDefaultAsync(e => e.UserId == userId && e.Id == id);
        }

        public async Task UpdateExpense(Expense expense)
        {
            _dbContext.Expenses.Update(expense);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteExpense(int id)
        {
            var expense = await _dbContext.Expenses.FindAsync(id);
            if (expense != null)
            {
                _dbContext.Expenses.Remove(expense);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}