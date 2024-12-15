using ICT.Strypes.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ICT.Strypes.Infrastructure.Data.EntitesConfiguration
{
    public class ChargePointEntityTypeConfiguration : IEntityTypeConfiguration<ChargePoint>
    {
        public void Configure(EntityTypeBuilder<ChargePoint> builder)
        {
            builder
           .HasKey(b => b.ChargePointId);

            builder
            .Property(b => b.ChargePointId)
            .HasMaxLength(39)
            .IsRequired();

            builder
            .Property(b => b.Status)
            .HasConversion<string>()
            .HasMaxLength(39)
            .IsRequired();

            builder
            .Property(b => b.FloorLevel)
            .HasMaxLength(4)
            .IsRequired(false);

            builder
            .Property(b => b.LastUpdated)
            .IsRequired();

            builder
            .HasOne(e => e.Location)
            .WithMany(e => e.ChargePoints)
            .HasForeignKey(e => e.LocationId)
            .IsRequired();
        }
    }
}
