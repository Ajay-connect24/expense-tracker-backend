using System.ComponentModel.DataAnnotations;

namespace transaction_tracker.Entities.Models
{
    public class Expense
    {
        [Key]
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public string Category { get; set; }
        public string? Note { get; set; }
    }
}