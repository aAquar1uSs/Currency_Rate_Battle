using CurrencyRateBattleServer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CurrencyRateBattleServer.Data.ModelConfigurations;

public class CurrencyConfiguration : IEntityTypeConfiguration<Currency>
{
    public void Configure(EntityTypeBuilder<Currency> builder)
    {
        _ = builder.ToTable("Currency")
            .HasKey(cur => cur.Id);
    }
}


