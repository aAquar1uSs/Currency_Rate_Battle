using CurrencyRateBattle_Server.Models;
using Microsoft.EntityFrameworkCore;

namespace CurrencyRateBattle_Server.Contexts;

public class AccountDbContext : DbContext
{
    private DbSet<Account> Accounts { get; }

    public AccountDbContext()
    {
        Database.EnsureCreated();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }
}
