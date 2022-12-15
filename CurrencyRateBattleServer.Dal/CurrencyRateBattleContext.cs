using CurrencyRateBattleServer.Dal.Entities;
using CurrencyRateBattleServer.Dal.ModelConfigurations;
using Microsoft.EntityFrameworkCore;

namespace CurrencyRateBattleServer.Dal;

public class CurrencyRateBattleContext : DbContext
{
    public DbSet<UserDal> Users { get; set; } = default!;

    public DbSet<AccountDal> Accounts { get; set; } = default!;

    public DbSet<AccountHistoryDal> AccountHistory { get; set; } = default!;

    public DbSet<RateDal> Rates { get; set; } = default!;

    public DbSet<RoomDal> Rooms { get; set; } = default!;

    public DbSet<CurrencyDal> Currencies { get; set; } = default!;

    public DbSet<CurrencyStateDal> CurrencyStates { get; set; } = default!;

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
        _ = modelBuilder.Entity<CurrencyDal>()
            .HasData(new CurrencyDal
            {
                Id = Guid.NewGuid(),
                CurrencyName = "USD",
                CurrencyCode = "$",
                Description = "US Dollar"
            },
            new CurrencyDal
            {
                Id = Guid.NewGuid(),
                CurrencyName = "EUR",
                CurrencyCode = "$",
                Description = "Euro"
            },
            new CurrencyDal
            {
                Id = Guid.NewGuid(),
                CurrencyName = "PLN",
                CurrencyCode = "zł",
                Description = "Polish Zlotych"
            },
            new CurrencyDal
            {
                Id = Guid.NewGuid(),
                CurrencyName = "GBP",
                CurrencyCode = "£",
                Description = "British Pound"
            },
            new CurrencyDal
            {
                Id = Guid.NewGuid(),
                CurrencyName = "CHF",
                CurrencyCode = "Fr",
                Description = "Swiss Franc"
            });

        //unique constraints
        _ = modelBuilder.Entity<CurrencyStateDal>()
            .HasIndex(cs => new { cs.RoomId, cs.CurrencyId })
            .IsUnique(true);
    }
}
