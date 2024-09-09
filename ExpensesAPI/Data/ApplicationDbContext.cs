using ExpensesAPI.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace ExpensesAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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

            // Seed predefined categories using PredefinedCategories
            SeedPredefinedCategories(modelBuilder);
        }

        private void SeedPredefinedCategories(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = PredefinedCategories.Food, UserId = 0 },
                new Category { Id = 2, Name = PredefinedCategories.Entertainment, UserId = 0 },
                new Category { Id = 3, Name = PredefinedCategories.Transportation, UserId = 0 },
                new Category { Id = 4, Name = PredefinedCategories.Utilities, UserId = 0 },
                new Category { Id = 5, Name = PredefinedCategories.Shopping, UserId = 0 },
                new Category { Id = 6, Name = PredefinedCategories.Other, UserId = 0 }
            );
        }
    }
}