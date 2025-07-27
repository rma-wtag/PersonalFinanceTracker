using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonalFinanceTracker.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        [Range(0,double.MaxValue)]
        public decimal Balance { get; set; }
        public ICollection<Transaction>? Transactions { get; set; } // 1:n
    }
}
