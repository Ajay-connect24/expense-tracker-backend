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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure the Amount property of the Expense entity
            modelBuilder.Entity<Expense>()
                .Property(e => e.Amount)
                .HasPrecision(18, 2); // Set precision and scale for decimal

            // You can add additional configurations for other entities here
        }
    }
}