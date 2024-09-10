using Microsoft.EntityFrameworkCore;
using transaction_tracker.Contracts;
using transaction_tracker.Data;
using transaction_tracker.Entities.Models;
using transaction_tracker.Entities.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mapster;

namespace transaction_tracker.Repository
{
    public class IncomeRepository : IIncomeRepository
    {
        private readonly TransactionDbContext _dbContext;

        public IncomeRepository(TransactionDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<TransactionWithIdDto>> GetLast10Incomes(Guid userId)
        {
            var incomes = await _dbContext.Incomes
                .Where(i => i.UserId == userId)
                .OrderByDescending(i => i.Date)
                .Take(10)
                .ToListAsync();

            return incomes.Adapt<List<TransactionWithIdDto>>();
        }

        public async Task<decimal> GetTotalMonthlyIncome(Guid userId, int month, int year)
        {
            var total = await _dbContext.Incomes
                .Where(i => i.UserId == userId && i.Date.Month == month && i.Date.Year == year)
                .SumAsync(i => i.Amount);

            return total;
        }

        public async Task<Dictionary<string, decimal>> GetMonthlyIncomes(Guid userId)
        {
            var monthlyIncomes = await _dbContext.Incomes
                .Where(i => i.UserId == userId)
                .GroupBy(i => new { i.Date.Month, i.Date.Year })
                .Select(g => new
                {
                    MonthYear = $"{g.Key.Month}/{g.Key.Year}",
                    TotalAmount = g.Sum(i => i.Amount)
                })
                .ToDictionaryAsync(g => g.MonthYear, g => g.TotalAmount);

            return monthlyIncomes;
        }

        public async Task<Dictionary<int, decimal>> GetDailyIncomes(Guid userId, int month, int year)
        {
            var dailyIncomes = await _dbContext.Incomes
                .Where(i => i.UserId == userId && i.Date.Month == month && i.Date.Year == year)
                .GroupBy(i => i.Date.Day)
                .Select(g => new
                {
                    Day = g.Key,
                    TotalAmount = g.Sum(i => i.Amount)
                })
                .ToDictionaryAsync(g => g.Day, g => g.TotalAmount);

            return dailyIncomes;
        }

        public async Task<Dictionary<string, decimal>> GetIncomeCategory(Guid userId, int month, int year)
        {
            var categoryIncomes = await _dbContext.Incomes
                .Where(i => i.UserId == userId && i.Date.Month == month && i.Date.Year == year)
                .GroupBy(i => i.Category)
                .Select(g => new
                {
                    Category = g.Key,
                    TotalAmount = g.Sum(i => i.Amount)
                })
                .ToDictionaryAsync(g => g.Category, g => g.TotalAmount);

            return categoryIncomes;
        }

        public async Task CreateIncome(Income income)
        {
            await _dbContext.Incomes.AddAsync(income);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Income> GetIncomeById(Guid userId, int id)
        {
            return await _dbContext.Incomes
                .FirstOrDefaultAsync(i => i.UserId == userId && i.Id == id);
        }

        public async Task UpdateIncome(Income income)
        {
            _dbContext.Incomes.Update(income);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteIncome(int id)
        {
            var income = await _dbContext.Incomes.FindAsync(id);
            if (income != null)
            {
                _dbContext.Incomes.Remove(income);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}