namespace PersonalFinanceTracker.Models
{
    public class Payment
    {
        public int PaymentId { get; set; }
        public PaymentStatus Status { get; set; }
        
        public int TransactionId { get; set; }
        public Transaction? Transaction { get; set; }
    }

    public enum PaymentStatus {
        Pending,
        Failed,
        Completed
    }
}
