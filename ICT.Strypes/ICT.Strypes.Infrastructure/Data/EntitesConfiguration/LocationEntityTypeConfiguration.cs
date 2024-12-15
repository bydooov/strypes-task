using ICT.Strypes.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ICT.Strypes.Infrastructure.Data.EntitesConfiguration
{
    public class LocationEntityTypeConfiguration : IEntityTypeConfiguration<Location>
    {
        public void Configure(EntityTypeBuilder<Location> builder)
        {
            builder
            .HasKey(b => b.LocationId);

            builder
            .Property(b => b.LocationId)
            .HasMaxLength(39)
            .IsRequired();

            builder
            .Property(b => b.Type)
            .HasConversion<string>()
            .HasMaxLength(45)
            .IsRequired();

            builder
            .Property(b => b.Name)
            .HasMaxLength(255)
            .IsRequired(false);

            builder
            .Property(b => b.Address)
            .HasMaxLength(45)
            .IsRequired();

            builder
            .Property(b => b.City)
            .HasMaxLength(45)
            .IsRequired();

            builder
            .Property(b => b.PostalCode)
            .HasMaxLength(10)
            .IsRequired();

            builder
            .Property(b => b.Country)
            .HasMaxLength(45)
            .IsRequired();

            builder
            .Property(b => b.LastUpdated)
            .IsRequired();
        }
    }
}
