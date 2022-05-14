using CurrencyRateBattle_Server.Models;
using Microsoft.EntityFrameworkCore;

namespace CurrencyRateBattle_Server.Contexts;

public class CurrencyRateBattleContext : DbContext
{
    private DbSet<Account> Accounts { get; }

    public CurrencyRateBattleContext()
    {
        Database.EnsureCreated();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
