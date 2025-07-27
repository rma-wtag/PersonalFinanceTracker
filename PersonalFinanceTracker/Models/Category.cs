namespace PersonalFinanceTracker.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public PaymentType PaymentType { get; set; }
        public ICollection<Transaction>? Transactions { get; set; } // Navigation to Transaction 1:n
    }

    public enum PaymentType
    {
        Expense,
        Income
    }
}
