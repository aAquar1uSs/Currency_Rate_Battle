using CurrencyRateBattleServer.Data.ModelConfigurations;
using CurrencyRateBattleServer.Models;
using Microsoft.EntityFrameworkCore;

namespace CurrencyRateBattleServer.Data;

public class CurrencyRateBattleContext : DbContext
{
    public DbSet<User> Users { get; set; }

    public DbSet<Account> Accounts { get; set; }

    public DbSet<AccountHistory> AccountHistory { get; set; }

    public DbSet<Rate> Rates { get; set; }

    public DbSet<Room> Rooms { get; set; }

    public DbSet<Currency> Currencies { get; set; }

    public DbSet<CurrencyState> CurrencyStates { get; set; }

    public CurrencyRateBattleContext(DbContextOptions<CurrencyRateBattleContext> options)
    : base(options)
    {
        if (options == null)
        {
            throw new ArgumentNullException(nameof(options));
        }
        //Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CurrencyStateConfiguration());
        modelBuilder.ApplyConfiguration(new RoomConfiguration());
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new AccountConfiguration());
        modelBuilder.ApplyConfiguration(new RateConfiguration());
        modelBuilder.ApplyConfiguration(new AccountHistoryConfiguration());
        modelBuilder.ApplyConfiguration(new CurrencyConfiguration());

        //seeding
        _ = modelBuilder.Entity<Currency>()
            .HasData(new Currency
            {
                Id = Guid.NewGuid(),
                CurrencyName = "USD",
                CurrencySymbol = "$",
                Description = "US Dollar"
            },
            new Currency
            {
                Id = Guid.NewGuid(),
                CurrencyName = "EUR",
                CurrencySymbol = "$",
                Description = "Euro"
            },
            new Currency
            {
                Id = Guid.NewGuid(),
                CurrencyName = "PLN",
                CurrencySymbol = "zł",
                Description = "Polish Zlotych"
            },
            new Currency
            {
                Id = Guid.NewGuid(),
                CurrencyName = "GBP",
                CurrencySymbol = "£",
                Description = "British Pound"
            },
            new Currency
            {
                Id = Guid.NewGuid(),
                CurrencyName = "CHF",
                CurrencySymbol = "Fr",
                Description = "Swiss Franc"
            });

    }
}
