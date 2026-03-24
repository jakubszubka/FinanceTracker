using FinanceTracker.Application.DTOs;
using FinanceTracker.Application.Interfaces;
using FinanceTracker.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FinanceTracker.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionsController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        private string GetUserId()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
        }

        [HttpGet]
        public async Task<IActionResult> GetUserTransactions()
        {
            var user = GetUserId();
            var transactions = await _transactionService.GetUserTransactions(user);

            return Ok(transactions);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserTransactionById(int id)
        {
            var user = GetUserId();
            var transaction = await _transactionService.GetById(id, user);
            if (transaction == null) return NotFound();

            return Ok(transaction);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTransaction(CreateTransactionDto dto)
        {
            var userId = GetUserId();
            TransactionDto? transaction;
            try
            {
                transaction = await _transactionService.CreateTransaction(dto, userId);
            }
            catch (Exception ex) { return BadRequest("Invalid transaction data"); }

            return CreatedAtAction(
                nameof(GetUserTransactionById),
                new { id = transaction.Id },
                transaction);

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTransaction(int id, UpdateTransactionDto dto)
        {
            var userId = GetUserId();
            var updatedTransaction = await _transactionService.UpdateTransaction(id, dto, userId);
            if (updatedTransaction == null) return NotFound();
            return Ok(updatedTransaction);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransaction(int id)
        {
            var userId = GetUserId();
            var deleted = await _transactionService.DeleteTransaction(id, userId);
            if(!deleted) return NotFound();
            return NoContent();
        }
    }
}
