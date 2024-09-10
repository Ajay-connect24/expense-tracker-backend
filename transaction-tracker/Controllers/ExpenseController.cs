using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using transaction_tracker.Contracts;
using transaction_tracker.Entities.DataTransferObjects;
using transaction_tracker.Entities.Models;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace transaction_tracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpenseController : ControllerBase
    {
        private readonly IExpenseRepository _expenseRepository;

        public ExpenseController(IExpenseRepository expenseRepository)
        {
            _expenseRepository = expenseRepository;
        }

        // GET: api/expense/last10
        [HttpGet("last10")]
        public async Task<ActionResult<List<TransactionDto>>> GetLast10Transactions()
        {
            var userId = GetUserIdFromClaims();
            var transactions = await _expenseRepository.GetLast10Transactions(userId);
            return Ok(transactions);
        }

        // GET: api/expense/totalmonthly/{month}/{year}
        [HttpGet("totalmonthly/{month}/{year}")]
        public async Task<ActionResult<decimal>> GetTotalMonthlyExpense(int month, int year)
        {
            var userId = GetUserIdFromClaims();
            var total = await _expenseRepository.GetTotalMonthlyExpense(userId, month, year);
            return Ok(total);
        }

        // GET: api/expense/monthlyexpenses
        [HttpGet("monthlyexpenses")]
        public async Task<ActionResult<Dictionary<string, decimal>>> GetMonthlyExpenses()
        {
            var userId = GetUserIdFromClaims();
            var monthlyExpenses = await _expenseRepository.GetMonthlyExpenses(userId);
            return Ok(monthlyExpenses);
        }

        // GET: api/expense/dailyexpenses/{month}/{year}
        [HttpGet("dailyexpenses/{month}/{year}")]
        public async Task<ActionResult<Dictionary<int, decimal>>> GetDailyExpenses(int month, int year)
        {
            var userId = GetUserIdFromClaims();
            var dailyExpenses = await _expenseRepository.GetDailyExpenses(userId, month, year);
            return Ok(dailyExpenses);
        }

        // GET: api/expense/categoryexpenses/{month}/{year}
        [HttpGet("categoryexpenses/{month}/{year}")]
        public async Task<ActionResult<Dictionary<string, decimal>>> GetExpenseCategory(int month, int year)
        {
            var userId = GetUserIdFromClaims();
            var categoryExpenses = await _expenseRepository.GetExpenseCategory(userId, month, year);
            return Ok(categoryExpenses);
        }

        // POST: api/expense
        [HttpPost]
        public async Task<ActionResult<TransactionDto>> CreateExpense([FromBody] TransactionDto transactionDto)
        {
            if (transactionDto == null)
            {
                return BadRequest("Expense data is null.");
            }

            var expense = transactionDto.Adapt<Expense>();
            expense.UserId = GetUserIdFromClaims(); // Set the UserId from claims

            await _expenseRepository.CreateExpense(expense);
            var createdExpense = await _expenseRepository.GetExpenseById(expense.UserId, expense.Id);
            return CreatedAtAction(nameof(GetExpenseById), new { id = createdExpense.Id }, createdExpense.Adapt<TransactionDto>());
        }

        // GET: api/expense/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<TransactionDto>> GetExpenseById(int id)
        {
            var userId = GetUserIdFromClaims();
            var expense = await _expenseRepository.GetExpenseById(userId, id);

            if (expense == null)
            {
                return NotFound();
            }

            return Ok(expense.Adapt<TransactionDto>());
        }

        // PUT: api/expense/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateExpense(int id, [FromBody] TransactionDto transactionDto)
        {
            if (transactionDto == null)
            {
                return BadRequest("Expense data is invalid.");
            }

            var userId = GetUserIdFromClaims();
            var existingExpense = await _expenseRepository.GetExpenseById(userId, id);

            if (existingExpense == null)
            {
                return NotFound();
            }

            existingExpense.Adapt(transactionDto);
            await _expenseRepository.UpdateExpense(existingExpense);
            return NoContent();
        }

        // DELETE: api/expense/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteExpense(int id)
        {
            var userId = GetUserIdFromClaims();
            var existingExpense = await _expenseRepository.GetExpenseById(userId, id);

            if (existingExpense == null)
            {
                return NotFound();
            }

            await _expenseRepository.DeleteExpense(id);
            return NoContent();
        }

        private Guid GetUserIdFromClaims()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            return Guid.Parse(userIdClaim.Value);
        }
    }
}