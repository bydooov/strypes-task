using ICT.Strypes.Domain.Entities;
using ICT.Strypes.Infrastructure.Data;
using ICT.Strypes.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace ICT.Strypes.UnitTests.RepositoryTests
{
    public class ChargePointRepositoryTests
    {
        private readonly Mock<ILogger<ChargePointRepository>> _mockLogger;
        private readonly ApplicationDbContext _context;
        private readonly ChargePointRepository _repository;

        public ChargePointRepositoryTests()
        {
            _mockLogger = new Mock<ILogger<ChargePointRepository>>();
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);

            _repository = new ChargePointRepository(_context, _mockLogger.Object);
        }

        [Fact]
        public async Task CreateChargePointAsync_ShouldAddChargePointToDatabase()
        {
            // Arrange
            var chargePoint = new ChargePoint
            {
                ChargePointId = "Point1",
                Status = ChargePointStatus.Available,
                FloorLevel = "2",
                LocationId = "Location1",
                LastUpdated = DateTime.Now
            };

            // Act
            await _repository.CreateChargePointAsync(chargePoint);

            // Assert
            var savedChargePoint = await _context.ChargePoints.FirstOrDefaultAsync(c => c.ChargePointId == chargePoint.ChargePointId);
            Assert.NotNull(savedChargePoint);
            Assert.Equal(chargePoint.ChargePointId, savedChargePoint.ChargePointId);
            Assert.Equal(chargePoint.Status, savedChargePoint.Status);
            Assert.Equal(chargePoint.FloorLevel, savedChargePoint.FloorLevel);
            Assert.Equal(chargePoint.LocationId, savedChargePoint.LocationId);
            Assert.Equal(chargePoint.LastUpdated, savedChargePoint.LastUpdated);
        }

        [Fact]
        public async Task UpdateChargePointAsync_ShouldUpdateChargePointInDatabase()
        {
            // Arrange
            var chargePoint = new ChargePoint
            {
                ChargePointId = "Point1",
                Status = ChargePointStatus.Available,
                FloorLevel = "2",
                LocationId = "Location1",
                LastUpdated = DateTime.Now
            };

            _context.ChargePoints.Add(chargePoint);
            await _context.SaveChangesAsync();

            chargePoint.Status = ChargePointStatus.Removed;
            chargePoint.FloorLevel = "3";

            // Act
            await _repository.UpdateChargePointAsync(chargePoint);

            // Assert
            var updatedChargePoint = await _context.ChargePoints.FirstOrDefaultAsync(c => c.ChargePointId == chargePoint.ChargePointId);
            Assert.NotNull(updatedChargePoint);
            Assert.Equal(chargePoint.Status, updatedChargePoint.Status);
            Assert.Equal(chargePoint.FloorLevel, updatedChargePoint.FloorLevel);
        }
    }
}

