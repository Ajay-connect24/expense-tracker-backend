

namespace ExpensesAPI.Entities.DataTransferObjects
{
    public class TransactionDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public int CategoryId { get; set; }
        public bool IsIncome { get; set; }
    }
}