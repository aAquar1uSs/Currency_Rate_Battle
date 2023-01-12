using CurrencyRateBattleServer.Dal.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CurrencyRateBattleServer.Dal.ModelConfigurations;

public class RoomConfiguration : IEntityTypeConfiguration<RoomDal>
{
    public void Configure(EntityTypeBuilder<RoomDal> builder)
    {
        _ = builder.ToTable("Room")
            .HasKey(room => room.Id);
        _ = builder.ToTable("Room")
            .Property(r => r.IsClosed)
            .HasDefaultValue(0);
    }
}


