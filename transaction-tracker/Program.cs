using transaction_tracker.Data;
using Microsoft.EntityFrameworkCore;
using transaction_tracker.Contracts;
using transaction_tracker.Repository;

var builder = WebApplication.CreateBuilder(args);

ConfigurationManager configuration = builder.Configuration;

builder.Services.AddDbContext<TransactionDbContext>(options =>
{
    options.UseSqlServer(configuration.GetConnectionString("ConnStr"));
});

builder.Services.AddScoped<IExpenseRepository, ExpenseRepository>();

// Add services to the container.

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Run();
