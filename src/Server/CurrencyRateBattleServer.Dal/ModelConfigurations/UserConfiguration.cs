using CurrencyRateBattleServer.Dal.Entities;
using CurrencyRateBattleServer.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CurrencyRateBattleServer.Dal.ModelConfigurations;

public class UserConfiguration : IEntityTypeConfiguration<UserDal>
{
    public void Configure(EntityTypeBuilder<UserDal> builder)
    {
        _ = builder.ToTable("User").HasKey(user => user.Email);
    }
}
