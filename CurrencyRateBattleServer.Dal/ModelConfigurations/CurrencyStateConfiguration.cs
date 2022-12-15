using CurrencyRateBattleServer.Dal.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CurrencyRateBattleServer.Dal.ModelConfigurations;

public class CurrencyStateConfiguration : IEntityTypeConfiguration<CurrencyStateDal>
{
    public void Configure(EntityTypeBuilder<CurrencyStateDal> builder)
    {
        _ = builder.ToTable("CurrencyState").HasKey(state => state.Id);
        _ = builder.ToTable("CurrencyState").HasOne(dal => dal.Room)
            .WithOne(dal => dal.CurrencyState)
            .HasForeignKey<RoomDal>(dal => dal.CurrencyStateId);
        //ToDo Add configuration for Currency
    }
}
