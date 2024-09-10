using Microsoft.EntityFrameworkCore;
using transaction_tracker.Entities.Models;

namespace transaction_tracker.Data
{

    public class TransactionDbContext : DbContext
    {
        public DbSet<Income> Incomes { get; set; }
        public DbSet<Expense> Expenses { get; set; }

        public TransactionDbContext(DbContextOptions<TransactionDbContext> options) : base(options)
        {
        }

    }
}
