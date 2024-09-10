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
using Microsoft.AspNetCore.Authorization;

namespace transaction_tracker.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class IncomeController : ControllerBase
    {
        private readonly IIncomeRepository _incomeRepository;

        public IncomeController(IIncomeRepository incomeRepository)
        {
            _incomeRepository = incomeRepository;
        }

        [HttpGet("last10")]
        public async Task<ActionResult<List<TransactionDto>>> GetLast10Incomes()
        {
            var userId = GetUserIdFromClaims();
            var incomes = await _incomeRepository.GetLast10Incomes(userId);
            return Ok(incomes);
        }

        [HttpGet("totalmonthly/{month}/{year}")]
        public async Task<ActionResult<decimal>> GetTotalMonthlyIncome(int month, int year)
        {
            var userId = GetUserIdFromClaims();
            var total = await _incomeRepository.GetTotalMonthlyIncome(userId, month, year);
            return Ok(total);
        }

        [HttpGet("monthlyincomes")]
        public async Task<ActionResult<Dictionary<string, decimal>>> GetMonthlyIncomes()
        {
            var userId = GetUserIdFromClaims();
            var monthlyIncomes = await _incomeRepository.GetMonthlyIncomes(userId);
            return Ok(monthlyIncomes);
        }

        [HttpGet("dailyincomes/{month}/{year}")]
        public async Task<ActionResult<Dictionary<int, decimal>>> GetDailyIncomes(int month, int year)
        {
            var userId = GetUserIdFromClaims();
            var dailyIncomes = await _incomeRepository.GetDailyIncomes(userId, month, year);
            return Ok(dailyIncomes);
        }

        [HttpGet("categoryincomes/{month}/{year}")]
        public async Task<ActionResult<Dictionary<string, decimal>>> GetIncomeCategory(int month, int year)
        {
            var userId = GetUserIdFromClaims();
            var categoryIncomes = await _incomeRepository.GetIncomeCategory(userId, month, year);
            return Ok(categoryIncomes);
        }

        [HttpPost]
        public async Task<ActionResult<TransactionDto>> CreateIncome([FromBody] TransactionDto transactionDto)
        {
            if (transactionDto == null)
            {
                return BadRequest("Income data is null.");
            }

            var income = transactionDto.Adapt<Income>();
            income.UserId = GetUserIdFromClaims(); // Set the UserId from claims

            await _incomeRepository.CreateIncome(income);
            var createdIncome = await _incomeRepository.GetIncomeById(income.UserId, income.Id);
            return CreatedAtAction(nameof(GetIncomeById), new { id = createdIncome.Id }, createdIncome.Adapt<TransactionDto>());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TransactionDto>> GetIncomeById(int id)
        {
            var userId = GetUserIdFromClaims();
            var income = await _incomeRepository.GetIncomeById(userId, id);

            if (income == null)
            {
                return NotFound();
            }

            return Ok(income.Adapt<TransactionDto>());
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateIncome(int id, [FromBody] TransactionDto transactionDto)
        {
            if (transactionDto == null)
            {
                return BadRequest("Income data is invalid.");
            }

            var userId = GetUserIdFromClaims();
            var existingIncome = await _incomeRepository.GetIncomeById(userId, id);

            if (existingIncome == null)
            {
                return NotFound();
            }

            existingIncome.Adapt(transactionDto);
            await _incomeRepository.UpdateIncome(existingIncome);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteIncome(int id)
        {
            var userId = GetUserIdFromClaims();
            var existingIncome = await _incomeRepository.GetIncomeById(userId, id);

            if (existingIncome == null)
            {
                return NotFound();
            }

            await _incomeRepository.DeleteIncome(id);
            return NoContent();
        }

        private Guid GetUserIdFromClaims()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null || string.IsNullOrEmpty(userIdClaim.Value))
            {
                throw new InvalidOperationException("User ID claim not found.");
            }

            return Guid.Parse(userIdClaim.Value);
        }
    }
}