using CurrencyRateBattleServer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CurrencyRateBattleServer.Data.ModelConfigurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        _ = builder.ToTable("User").HasKey(user => user.Id);
        _ = builder.Property(user => user.Email).IsRequired();
        _ = builder.ToTable("User")
            .HasOne(user => user.Account)
            .WithOne(acc => acc.User)
            .HasForeignKey<Account>(acc => acc.UserId);
    }
}
