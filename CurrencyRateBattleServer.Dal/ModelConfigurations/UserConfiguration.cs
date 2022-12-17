using CurrencyRateBattleServer.Dal.Entities;
using CurrencyRateBattleServer.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CurrencyRateBattleServer.Dal.ModelConfigurations;

public class UserConfiguration : IEntityTypeConfiguration<UserDal>
{
    public void Configure(EntityTypeBuilder<UserDal> builder)
    {
        _ = builder.ToTable("User").HasKey(user => user.Id);
        _ = builder.Property(user => user.Email).IsRequired();
        _ = builder.ToTable("User")
            .HasOne(user => user.Account)
            .WithOne(acc => acc.User)
            .HasForeignKey<AccountDal>(acc => acc.UserId);
    }
}
