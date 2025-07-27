using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonalFinanceTracker.Models
{
    public class Transaction
    {
        public int TransactionId { get; set; }
        public string? Description { get; set; }
        public DateTime Date { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        [Range(0, double.MaxValue)]
        public decimal Amount { get; set; }
        public int UserId { get; set; } // Foreign Key for user
        public User User { get; set; } // Navigation to user n:1
        public int CategoryId { get; set; } // Foreign Key
        public Category Category { get; set; } // Navigation to Category 1:1
        public Payment Payment { get; set; }
    }
}