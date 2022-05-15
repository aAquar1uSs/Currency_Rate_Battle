using CurrencyRateBattleServer.Models;
using Microsoft.EntityFrameworkCore;

namespace CurrencyRateBattleServer.Contexts;

public class CurrencyRateBattleContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Account> Accounts { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<CurrencyState> CurrencyStates { get; set; }

    public CurrencyRateBattleContext(DbContextOptions<CurrencyRateBattleContext> options)
    : base(options)
    {
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CurrencyState>(cs => cs.HasKey(k => k.Id));
        modelBuilder.Entity<Room>()
            .HasKey(room => room.Id);
        modelBuilder.Entity<Room>()
            .Property(r => r.IsClosed)
            .HasDefaultValue(0);


        modelBuilder.Entity<User>(acc => acc.HasKey(k => k.Id));
        modelBuilder.Entity<Account>(bill => bill.HasKey(b => b.Id));
        modelBuilder.Entity<User>()
            .HasOne(user => user.Bill)
            .WithOne(acc => acc.User)
            .HasForeignKey<Account>(bill => bill.UserRef);
    }
}
