using Lab4.Models;
using Microsoft.EntityFrameworkCore;

namespace Lab4.Repository;

public sealed class ApplicationContext : DbContext
{
    public DbSet<CalculatorState> States => Set<CalculatorState>();

    public ApplicationContext() => Database.EnsureCreated();
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=lab3.db");
    }
}