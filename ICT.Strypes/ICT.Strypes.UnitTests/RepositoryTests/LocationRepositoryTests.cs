using ICT.Strypes.Domain.Entities;
using ICT.Strypes.Infrastructure.Data;
using ICT.Strypes.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace ICT.Strypes.UnitTests.RepositoryTests
{
    public class LocationRepositoryTests
    {
        private readonly Mock<ILogger<LocationRepository>> _mockLogger;
        private readonly ApplicationDbContext _context;
        private readonly LocationRepository _repository;

        public LocationRepositoryTests()
        {
            _mockLogger = new Mock<ILogger<LocationRepository>>();
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);

            _repository = new LocationRepository(_context, _mockLogger.Object);
        }

        [Fact]
        public async Task CreateLocationAsync_ShouldAddLocationToDatabase()
        {
            // Arrange
            var location = new Location
            {
                LocationId = "Location1",
                LastUpdated = DateTime.Now,
                Address = "Test",
                City = "Test",
                PostalCode = "Test",
                Type = LocationType.Airport,
                Country = "Test"
            };

            // Act
            await _repository.CreateLocationAsync(location);

            // Assert
            var savedLocation = await _context.Locations.FirstOrDefaultAsync(l => l.LocationId == location.LocationId);
            Assert.NotNull(savedLocation);
            Assert.Equal(location.LocationId, savedLocation.LocationId);
            Assert.Equal(location.LastUpdated, savedLocation.LastUpdated);
            Assert.Equal(location.Address, savedLocation.Address);
            Assert.Equal(location.Country, savedLocation.Country);
            Assert.Equal(location.City, savedLocation.City);
            Assert.Equal(location.Type, savedLocation.Type);
            Assert.Equal(location.PostalCode, savedLocation.PostalCode);
        }

        [Fact]
        public async Task UpdateLocationAsync_ShouldUpdateLocationInDatabase()
        {
            // Arrange
            var location = new Location
            {
                LocationId = "Location1",
                LastUpdated = DateTime.Now,
                Address = "Test",
                City = "Test",
                PostalCode = "Test",
                Type = LocationType.Airport,
                Country = "Test"
            };

            _context.Locations.Add(location);
            await _context.SaveChangesAsync();

            location.Name = "NewName";

            // Act
            await _repository.UpdateLocationAsync(location);

            // Assert
            var updatedLocation = await _context.Locations.FirstOrDefaultAsync(l => l.LocationId == location.LocationId);
            Assert.NotNull(updatedLocation);
            Assert.Equal(location.Name, updatedLocation.Name);
        }

        [Fact]
        public async Task GetLocationAsync_ShouldGetLocationInDatabase()
        {
            // Arrange
            var location = new Location
            {
                LocationId = "Location1",
                LastUpdated = DateTime.Now,
                Address = "Test",
                City = "Test",
                PostalCode = "Test",
                Type = LocationType.Airport,
                Country = "Test"
            };

            _context.Locations.Add(location);
            await _context.SaveChangesAsync();

            // Act
            var existingLocation = await _repository.GetLocationAsync(location.LocationId);

            // Assert
            Assert.NotNull(existingLocation);
            Assert.Equal(location.Name, existingLocation.Name);
            Assert.Equal(location.LocationId, existingLocation.LocationId);
            Assert.Equal(location.LastUpdated, existingLocation.LastUpdated);
            Assert.Equal(location.Address, existingLocation.Address);
            Assert.Equal(location.Country, existingLocation.Country);
            Assert.Equal(location.City, existingLocation.City);
            Assert.Equal(location.Type, existingLocation.Type);
            Assert.Equal(location.PostalCode, existingLocation.PostalCode);
        }
    }
}
