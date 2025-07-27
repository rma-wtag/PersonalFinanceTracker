using PersonalFinanceTracker.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonalFinanceTracker.Dtos.TransactionDtos
{
    public class CreateTransactionDto
    {
        [Required]
        public string Description { get; set; }
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0, double.MaxValue)]
        public decimal Amount { get; set; }
        [Required]
        public int UserId { get; set; } // Foreign Key for user
        [Required]
        public int CategoryId { get; set; }
    }
}
