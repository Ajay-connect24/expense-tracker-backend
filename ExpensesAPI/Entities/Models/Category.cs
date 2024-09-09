namespace ExpensesAPI.Entities.Models
{
    public class Category
    {
        public int Id { get; set; } 
        public string Name { get; set; } 
        public int UserId { get; set; } 
        public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}
