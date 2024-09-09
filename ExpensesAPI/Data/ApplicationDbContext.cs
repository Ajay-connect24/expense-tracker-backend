using ExpensesAPI.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace ExpensesAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure the one-to-many relationship between User and Transaction
            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.User) // Each transaction has one user
                .WithMany(u => u.Transactions) // A user can have many transactions
                .HasForeignKey(t => t.UserId) // Foreign key in Transaction
                .OnDelete(DeleteBehavior.Cascade); // Optional: Set to cascade delete if needed

            // Configure the one-to-many relationship between Category and Transaction
            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Category) // Each transaction has one category
                .WithMany(c => c.Transactions) // A category can have many transactions
                .HasForeignKey(t => t.CategoryId) // Foreign key in Transaction
                .OnDelete(DeleteBehavior.Cascade); // Optional: Set to cascade delete if needed

            // Configure the decimal property 'Amount' in Transaction
            modelBuilder.Entity<Transaction>()
                .Property(t => t.Amount)
                .HasPrecision(18, 2); // Specify precision and scale for decimal type
        }
    }
}