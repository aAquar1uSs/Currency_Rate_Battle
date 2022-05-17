using CurrencyRateBattleServer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CurrencyRateBattleServer.Contexts.ModelConfigurations;

public class CurrencyStateConfiguration : IEntityTypeConfiguration<CurrencyState>
{
    public void Configure(EntityTypeBuilder<CurrencyState> builder)
    {
        builder.ToTable("CurrencyState").HasKey(state => state.Id);
    }
}
