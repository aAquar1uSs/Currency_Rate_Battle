using CurrencyRateBattleServer.Dal.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CurrencyRateBattleServer.Dal.ModelConfigurations;

public class RateConfiguration : IEntityTypeConfiguration<RateDal>
{
    public void Configure(EntityTypeBuilder<RateDal> builder)
    {
        _ = builder.ToTable("Rate")
            .HasKey(rate => rate.Id);
        _ = builder.ToTable("Rate")
            .Property(r => r.IsClosed)
            .HasDefaultValue(0);
    }
}


