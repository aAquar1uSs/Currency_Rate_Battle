using CurrencyRateBattleServer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CurrencyRateBattleServer.Contexts.ModelConfigurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("User").HasKey(user => user.Id);
        builder.Property(user => user.Email).IsRequired();
        builder.ToTable("User")
            .HasOne(user => user.Bill)
            .WithOne(acc => acc.User)
            .HasForeignKey<Account>(bill => bill.UserRef);
    }
}
