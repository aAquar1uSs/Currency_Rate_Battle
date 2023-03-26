using CurrencyRateBattleServer.Dal.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CurrencyRateBattleServer.Dal.ModelConfigurations;

public class CurrencyConfiguration : IEntityTypeConfiguration<CurrencyDal>
{
    public void Configure(EntityTypeBuilder<CurrencyDal> builder)
    {
        _ = builder.ToTable("Currency")
            .HasKey(cur => cur.CurrencyName);

        _ = builder.ToTable("Currency")
            .Property(currency => currency.UpdateDate)
            .HasDefaultValue(DateTime.UtcNow);
        
        _ = builder.ToTable("Currency")
            .Property(currency => currency.Rate)
            .HasDefaultValue(0);
    }
}


