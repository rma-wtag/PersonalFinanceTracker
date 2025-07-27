using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PersonalFinanceTracker.Dtos.TransactionDtos;
using PersonalFinanceTracker.Entities;
using PersonalFinanceTracker.Models;

namespace PersonalFinanceTracker.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public TransactionController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Transaction>> GetTransactionById([FromRoute] int id) {
            var transaction = await _context.Transactions.FirstOrDefaultAsync(t => t.TransactionId == id);

            if (transaction == null) return NotFound();
            return Ok(transaction);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateTransaction([FromBody] CreateTransactionDto transactionDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == transactionDto.UserId);
                var category = await _context.Categories.FirstOrDefaultAsync(c => c.CategoryId == transactionDto.CategoryId);

                if (user == null) return NotFound("User not found.");
                if (category == null) return NotFound("Category not found.");

                var createdTransaction = new Transaction
                {
                    Description = transactionDto.Description,
                    Date = DateTime.UtcNow,
                    Amount = transactionDto.Amount,
                    UserId = transactionDto.UserId,
                    CategoryId = transactionDto.CategoryId
                };

                var payment = new Payment
                {
                    Status = PaymentStatus.Pending
                };

                createdTransaction.Payment = payment;

                await _context.Transactions.AddAsync(createdTransaction);

                if (category.PaymentType == PaymentType.Expense)
                {
                    user.Balance -= createdTransaction.Amount;
                }
                else { 
                    user.Balance += createdTransaction.Amount;
                }

                payment.Status = user.Balance >= 0 ? PaymentStatus.Completed : PaymentStatus.Failed;

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                var fullTransaction = await _context.Transactions
                                            .Include(t => t.Payment)
                                            .Include(t => t.User)
                                            .Include(t => t.Category)
                                            .FirstOrDefaultAsync(t => t.TransactionId == createdTransaction.TransactionId);

                return CreatedAtAction(nameof(GetTransactionById), new { id = fullTransaction.TransactionId }, fullTransaction);
            }
            catch (Exception ex) {
                await transaction.RollbackAsync();
                return StatusCode(500, $"Error occured while completing the transaction : {ex.Message}");
            }
        }
    }
}

