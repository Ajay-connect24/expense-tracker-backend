using transaction_tracker.Entities.DataTransferObjects;
using transaction_tracker.Entities.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace transaction_tracker.Contracts
{
    public interface IIncomeRepository
    {
        Task<List<TransactionWithIdDto>> GetLast10Incomes(Guid userId);
        Task<decimal> GetTotalMonthlyIncome(Guid userId, int month, int year);
        Task<Dictionary<string, decimal>> GetMonthlyIncomes(Guid userId);
        Task<Dictionary<int, decimal>> GetDailyIncomes(Guid userId, int month, int year);
        Task<Dictionary<string, decimal>> GetIncomeCategory(Guid userId, int month, int year);
        Task CreateIncome(Income income);
        Task<Income> GetIncomeById(Guid userId, int id);
        Task UpdateIncome(Income income);
        Task DeleteIncome(int id);
    }
}