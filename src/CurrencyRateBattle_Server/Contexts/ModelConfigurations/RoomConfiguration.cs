using CurrencyRateBattleServer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CurrencyRateBattleServer.Contexts.ModelConfigurations;

public class RoomConfiguration : IEntityTypeConfiguration<Room>
{
    public void Configure(EntityTypeBuilder<Room> builder)
    {
        builder.ToTable("Room")
            .HasKey(room => room.Id);
        builder.ToTable("Room")
            .Property(r => r.IsClosed)
            .HasDefaultValue(0);
    }
}


