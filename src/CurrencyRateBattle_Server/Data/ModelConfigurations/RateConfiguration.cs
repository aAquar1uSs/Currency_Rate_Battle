using CurrencyRateBattleServer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CurrencyRateBattleServer.Data.ModelConfigurations;

public class RateConfiguration : IEntityTypeConfiguration<Rate>
{
    public void Configure(EntityTypeBuilder<Rate> builder)
    {
        _ = builder.ToTable("Rate")
            .HasKey(rate => rate.Id);
        _ = builder.ToTable("Rate")
            .Property(r => r.IsClosed)
            .HasDefaultValue(0);
    }
}


