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
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        _ = modelBuilder.ApplyConfiguration(new CurrencyStateConfiguration());
        _ = modelBuilder.ApplyConfiguration(new RoomConfiguration());
        _ = modelBuilder.ApplyConfiguration(new UserConfiguration());
        _ = modelBuilder.ApplyConfiguration(new AccountConfiguration());
        _ = modelBuilder.ApplyConfiguration(new RateConfiguration());
        _ = modelBuilder.ApplyConfiguration(new AccountHistoryConfiguration());
        _ = modelBuilder.ApplyConfiguration(new CurrencyConfiguration());

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
