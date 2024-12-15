using ICT.Strypes.Domain.Entities;
using ICT.Strypes.Infrastructure.Data.EntitesConfiguration;
using Microsoft.EntityFrameworkCore;

namespace ICT.Strypes.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Location> Locations { get; set; }
        public DbSet<ChargePoint> ChargePoints { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new LocationEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ChargePointEntityTypeConfiguration());
        }
    }
}
