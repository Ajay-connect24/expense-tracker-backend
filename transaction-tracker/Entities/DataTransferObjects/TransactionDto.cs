namespace transaction_tracker.Entities.DataTransferObjects
{
    public class TransactionDto
    {
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public string Category { get; set; }
        public string? Note { get; set; } = null;
    }
}
