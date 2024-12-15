using ICT.Strypes.Domain.Entities;
using ICT.Strypes.Infrastructure.Data;
using ICT.Strypes.Infrastructure.Exceptions;
using ICT.Strypes.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ICT.Strypes.Infrastructure.Repositories
{
    public class LocationRepository : ILocationRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<LocationRepository> _logger;

        public LocationRepository(ApplicationDbContext context, ILogger<LocationRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Location> CreateLocationAsync(Location location)
        {
            try
            {
                var entry = await _context.Locations.AddAsync(location);

                await _context.SaveChangesAsync();

                return entry.Entity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                throw new DatabaseException(ex.Message);
            }
        }

        public async Task<Location> GetLocationAsync(string locationId)
        {
            try
            {
                var location = await _context.Locations.Include(lo => lo.ChargePoints).FirstOrDefaultAsync(lo => lo.LocationId == locationId);

                return location!;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                throw new DatabaseException(ex.Message);
            }
        }

        public async Task<Location> UpdateLocationAsync(Location location)
        {
            try
            {
                _context.Locations.Update(location);

                await _context.SaveChangesAsync();

                return location;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                throw new DatabaseException(ex.Message);
            }
        }
    }
}
