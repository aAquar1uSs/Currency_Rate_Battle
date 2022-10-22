using CurrencyRateBattleServer.Dal.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CurrencyRateBattleServer.Dal.ModelConfigurations;

public class AccountHistoryConfiguration : IEntityTypeConfiguration<AccountHistoryDal>
{
    public void Configure(EntityTypeBuilder<AccountHistoryDal> builder)
    {
        _ = builder.ToTable("AccountHistory")
            .HasKey(acch => acch.Id);
    }
}


