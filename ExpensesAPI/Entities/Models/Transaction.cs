using System.ComponentModel.DataAnnotations;

namespace ExpensesAPI.Entities.Models
{
    public class Transaction
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; } 
        public DateTime Date { get; set; } = DateTime.Now;
        public decimal Amount { get; set; }
        public int CategoryId { get; set; } 
        public bool IsIncome { get; set; }
        public virtual Category Category { get; set; }

    }
}
