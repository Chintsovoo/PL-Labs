using Lab3.entity;
using Microsoft.EntityFrameworkCore;

namespace Lab3;

public sealed class ApplicationContext : DbContext
{
    public DbSet<CalculatorStateEntity> CalculatorStateEntities => Set<CalculatorStateEntity>();

    public ApplicationContext() => Database.EnsureCreated();
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=lab3.db");
    }
    
}