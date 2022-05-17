using CurrencyRateBattleServer.Contexts.ModelConfigurations;
using CurrencyRateBattleServer.Models;
using Microsoft.EntityFrameworkCore;

namespace CurrencyRateBattleServer.Data;

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
        modelBuilder.ApplyConfiguration(new CurrencyStateConfiguration());
        modelBuilder.ApplyConfiguration(new RoomConfiguration());
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new AccountConfiguration());
    }
}
