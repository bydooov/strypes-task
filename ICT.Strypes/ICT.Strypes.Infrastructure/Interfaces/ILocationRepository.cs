using ICT.Strypes.Domain.Entities;

namespace ICT.Strypes.Infrastructure.Interfaces
{
    public interface ILocationRepository
    {
        Task<Location> CreateLocationAsync(Location location);
        Task<Location> GetLocationAsync(string locationId);
        Task<Location> UpdateLocationAsync(Location location);
    }
}
