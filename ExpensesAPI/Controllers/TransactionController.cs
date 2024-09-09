
using ExpensesAPI.Contracts;
using ExpensesAPI.Entities.DataTransferObjects;
using ExpensesAPI.Entities.Models;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ExpensesAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TransactionController(ITransactionRepository transactionRepository, IHttpContextAccessor httpContextAccessor)
        {
            _transactionRepository = transactionRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTransaction([FromBody] TransactionDto transactionDto)
        {
            var userId = GetUserIdFromClaims();
            if (userId == null)
            {
                return Unauthorized();
            }

            var transaction = transactionDto.Adapt<Transaction>();
            transaction.UserId = userId.Value; // Set the UserId from claims

            var createdTransaction = await _transactionRepository.CreateTransactionAsync(transaction);
            return CreatedAtAction(nameof(GetAllTransactions), new { id = createdTransaction.Id }, createdTransaction.Adapt<TransactionDto>());
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTransactions()
        {
            var userId = GetUserIdFromClaims();
            if (userId == null)
            {
                return Unauthorized();
            }

            var transactions = await _transactionRepository.GetAllTransactionsAsync(userId.Value);
            return Ok(transactions.Adapt<IEnumerable<TransactionDto>>());
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTransaction(int id, [FromBody] TransactionDto transactionDto)
        {
            var userId = GetUserIdFromClaims();
            if (userId == null)
            {
                return Unauthorized();
            }

            var transaction = transactionDto.Adapt<Transaction>();
            transaction.Id = id; // Ensure the ID is set for the update
            transaction.UserId = userId.Value; // Set the UserId from claims

            var updatedTransaction = await _transactionRepository.UpdateTransactionAsync(transaction);
            return Ok(updatedTransaction.Adapt<TransactionDto>());
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransaction(int id)
        {
            var userId = GetUserIdFromClaims();
            if (userId == null)
            {
                return Unauthorized();
            }

            var result = await _transactionRepository.DeleteTransactionAsync(id, userId.Value);
            if (!result)
            {
                return NotFound(); // Transaction not found
            }

            return NoContent(); // Successfully deleted
        }

        [HttpGet("weekly")]
        public async Task<IActionResult> GetWeeklyTransactions()
        {
            var userId = GetUserIdFromClaims();
            if (userId == null)
            {
                return Unauthorized();
            }

            var transactions = await _transactionRepository.GetWeeklyTransactionsAsync(userId.Value);
            return Ok(transactions.Adapt<IEnumerable<TransactionDto>>());
        }

        [HttpGet("monthly")]
        public async Task<IActionResult> GetMonthlyTransactions()
        {
            var userId = GetUserIdFromClaims();
            if (userId == null)
            {
                return Unauthorized();
            }

            var transactions = await _transactionRepository.GetMonthlyTransactionsAsync(userId.Value);
            return Ok(transactions.Adapt<IEnumerable<TransactionDto>>());
        }

        [HttpGet("custom-range")]
        public async Task<IActionResult> GetCustomRangeTransactions(DateTime startDate, DateTime endDate)
        {
            var userId = GetUserIdFromClaims();
            if (userId == null)
            {
                return Unauthorized();
            }

            var transactions = await _transactionRepository.GetCustomRangeTransactionsAsync(userId.Value, startDate, endDate);
            return Ok(transactions.Adapt<IEnumerable<TransactionDto>>());
        }

        [HttpGet("daily")]
        public async Task<IActionResult> GetDailyTransactions()
        {
            var userId = GetUserIdFromClaims();
            if (userId == null)
            {
                return Unauthorized();
            }

            var transactions = await _transactionRepository.GetDailyTransactionsAsync(userId.Value);
            return Ok(transactions.Adapt<IEnumerable<TransactionDto>>());
        }

        private int? GetUserIdFromClaims()
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);
            return userIdClaim != null ? int.Parse(userIdClaim.Value) : (int?)null;
        }
    }
}