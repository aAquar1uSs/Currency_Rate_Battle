using CurrencyRateBattle_Server.Models;
using Microsoft.EntityFrameworkCore;

namespace CurrencyRateBattle_Server.Contexts;

public class CurrencyRateBattleContext : DbContext
{
    public DbSet<User> Users { get; set; }

    public DbSet<Account> Accounts { get; set;  }

    public CurrencyRateBattleContext(DbContextOptions<CurrencyRateBattleContext> options)
    : base(options)
    {
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(acc => acc.HasKey(k => k.Id));
        modelBuilder.Entity<Account>(bill => bill.HasKey(b => b.Id));
        modelBuilder.Entity<User>()
            .HasOne(user => user.Bill)
            .WithOne(acc => acc.User)
            .HasForeignKey<Account>(bill => bill.AccountRef);
    }
}
