using CurrencyRateBattleServer.Dal.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CurrencyRateBattleServer.Dal.ModelConfigurations;

public class AccountConfiguration : IEntityTypeConfiguration<AccountDal>
{
    public void Configure(EntityTypeBuilder<AccountDal> builder)
    {
        _ = builder.ToTable("Account").HasKey(acc => acc.Id);
        _ = builder.ToTable("Account")
            .HasOne(acc => acc.User)
            .WithOne(acc => acc.Account)
            .HasForeignKey<UserDal>(dal => dal.AccountId);
    }
}
