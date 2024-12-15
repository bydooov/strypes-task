using AutoMapper;
using Azure.Core;
using ICT.Strypes.Business.Exceptions;
using ICT.Strypes.Business.Interfaces;
using ICT.Strypes.Business.Models;
using ICT.Strypes.Business.Services;
using ICT.Strypes.Domain.Entities;
using ICT.Strypes.Infrastructure.Interfaces;
using Moq;

namespace ICT.Strypes.UnitTests.ServiceTests
{
    public class LocationServiceTests
    {
        private readonly Mock<ILocationRepository> _mockLocationRepository;
        private readonly Mock<IChargePointRepository> _mockChargePointRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly LocationService _service;

        public LocationServiceTests()
        {
            _mockLocationRepository = new Mock<ILocationRepository>();
            _mockChargePointRepository = new Mock<IChargePointRepository>();
            _mockMapper = new Mock<IMapper>();
            _service = new LocationService(_mockLocationRepository.Object, _mockMapper.Object, _mockChargePointRepository.Object);
        }

        #region GET Location

        [Fact]
        public async Task GetLocation_ThrowsNotFoundException_WhenLocationNotFound()
        {
            // Arrange
            string locationId = "Location1";

            _mockLocationRepository.Setup(repo => repo.GetLocationAsync(locationId))
                .ReturnsAsync((Location)null!);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<NotFoundException>(() =>
                _service.GetLocationByIdAsync(locationId));
            Assert.Equal(string.Format("Location with id: {0} not found.", locationId), exception.Message);
        }

        [Fact]
        public async Task GetLocation_ReturnsLocationModel_WhenLocationIsFound()
        {
            // Arrange
            string locationId = "Location1";
            var location = new Location { LocationId = locationId, Name = "TestName" };
            var locationModel = new LocationModel { LocationId = locationId, Name = "TestName" };

            _mockLocationRepository.Setup(repo => repo.GetLocationAsync(locationId))
               .ReturnsAsync(location);

            _mockMapper.Setup(mapper => mapper.Map<LocationModel>(location))
                .Returns(locationModel);

            // Act
            var result = await _service.GetLocationByIdAsync(locationId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(locationModel.LocationId, result.LocationId);
            Assert.Equal(locationModel.Name, result.Name);

            _mockLocationRepository.Verify(repo => repo.GetLocationAsync(locationId), Times.Once);
            _mockMapper.Verify(mapper => mapper.Map<LocationModel>(location), Times.Once);
        }

        #endregion

        #region POST Location

        [Fact]
        public async Task PostLocation_ThrowsConflictException_WhenLocationAlreadyExist()
        {
            // Arrange
            string locationId = "Location1";

            var locationRequest = new LocationRequestModel
            {
                LocationId = locationId,
                Name = "TestName"
            };

            var location = new Location
            {
                LocationId = locationId
            };

            _mockLocationRepository.Setup(repo => repo.GetLocationAsync(locationId))
                .ReturnsAsync(location);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ConflictException>(() =>
                _service.PostLocationAsync(locationRequest));
            Assert.Equal(string.Format("Location with id: {0} already exist.", locationRequest.LocationId), exception.Message);
        }

        [Fact]
        public async Task PostLocation_ReturnsLocationModel_WhenLocationIsSuccessfulyCreatedInTheDatabase()
        {
            // Arrange
            string locationId = "Location1";
            var locationModelRequest = new LocationRequestModel { LocationId = locationId, Name = "TestName" };
            var location = new Location { LocationId = locationId, Name = "TestName" };
            var locationModel = new LocationModel { LocationId = locationId, Name = "TestName" };

            _mockLocationRepository.Setup(repo => repo.GetLocationAsync(locationId))
                .ReturnsAsync((Location)null!);

            _mockMapper.Setup(mapper => mapper.Map<Location>(locationModelRequest))
                .Returns(location);

            _mockLocationRepository.Setup(repo => repo.CreateLocationAsync(location))
               .ReturnsAsync(location);

            _mockMapper.Setup(mapper => mapper.Map<LocationModel>(location))
              .Returns(locationModel);


            // Act
            var result = await _service.PostLocationAsync(locationModelRequest);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(locationModel.LocationId, result.LocationId);
            Assert.Equal(locationModel.Name, result.Name);

            _mockLocationRepository.Verify(repo => repo.GetLocationAsync(locationId), Times.Once);
            _mockMapper.Verify(mapper => mapper.Map<Location>(locationModelRequest), Times.Once);
            _mockLocationRepository.Verify(repo => repo.CreateLocationAsync(location), Times.Once);
            _mockMapper.Verify(mapper => mapper.Map<LocationModel>(location), Times.Once);
        }

        #endregion

        #region PATCH Location

        [Fact]
        public async Task PatchLocation_ThrowsNotFoundException_WhenLocationNotFound()
        {
            // Arrange
            string locationId = "Location1";

            var patchLocationRequest = new PatchLocationRequestModel
            {
                Name = "TestName"
            };

            var location = new Location
            {
                LocationId = locationId
            };

            _mockLocationRepository.Setup(repo => repo.GetLocationAsync(locationId))
                .ReturnsAsync((Location)null!);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<NotFoundException>(() =>
                _service.PatchLocationAsync(locationId, patchLocationRequest));
            Assert.Equal(string.Format("Location with id: {0} not found.", locationId), exception.Message);
        }

        [Fact]
        public async Task PatchLocation_ReturnsLocationModel_WhenLocationIsSuccessfulyUpdated()
        {
            // Arrange
            string locationId = "Location1";
            var patchLocationModelRequest = new PatchLocationRequestModel { Name = "NewName" };
            var location = new Location { LocationId = locationId, Name = "TestName" };
            var locationModel = new LocationModel { LocationId = locationId, Name = "NewName" };

            _mockLocationRepository.Setup(repo => repo.GetLocationAsync(locationId))
                .ReturnsAsync(location);

            _mockMapper.Setup(mapper => mapper.Map(patchLocationModelRequest, location));

            _mockLocationRepository.Setup(repo => repo.UpdateLocationAsync(location))
               .ReturnsAsync(location);

            _mockMapper.Setup(mapper => mapper.Map<LocationModel>(location))
              .Returns(locationModel);


            // Act
            var result = await _service.PatchLocationAsync(locationId, patchLocationModelRequest);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(locationModel.Name, result.Name);

            _mockLocationRepository.Verify(repo => repo.GetLocationAsync(locationId), Times.Once);
            _mockMapper.Verify(mapper => mapper.Map(patchLocationModelRequest, location), Times.Once);
            _mockLocationRepository.Verify(repo => repo.UpdateLocationAsync(location), Times.Once);
            _mockMapper.Verify(mapper => mapper.Map<LocationModel>(location), Times.Once);
        }

        #endregion

        #region PATCH Location

        [Fact]
        public async Task PutLocation_ThrowsNotFoundException_WhenLocationNotFound()
        {
            // Arrange
            string locationId = "Location1";

            var location = new Location
            {
                LocationId = locationId
            };

            _mockLocationRepository.Setup(repo => repo.GetLocationAsync(locationId))
                .ReturnsAsync((Location)null!);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<NotFoundException>(() =>
                _service.UpsertChargePointsAsync(locationId, It.IsAny<ChargePointRequestModel>()));
            Assert.Equal(string.Format("Location with id: {0} not found.", locationId), exception.Message);
        }

        [Fact]
        public async Task PutLocation_ReturnsLocationModel_WhenChargePointAreSuccessfulyUpserted()
        {
            var locationId = "location123";
            var chargePointRequest = new ChargePointRequestModel
            {
                ChargePoints = new List<ChargePointModel>
                {
                    new ChargePointModel { ChargePointId = "Point1", FloorLevel = "2", Status = "Available", LastUpdated = DateTime.Now },
                    new ChargePointModel { ChargePointId = "Point2", FloorLevel = "2", Status = "Available", LastUpdated = DateTime.Now}
                }
            };

            var existingLocation = new Location
            {
                ChargePoints = new List<ChargePoint>
                {
                    new ChargePoint { ChargePointId = "Point1", FloorLevel = "1" },
                    new ChargePoint { ChargePointId = "Point3", FloorLevel = "3", Status = ChargePointStatus.Available },
                }
            };

            var updatedLocation = new Location
            {
                ChargePoints = new List<ChargePoint>
                {
                    new ChargePoint { ChargePointId = "Point1", FloorLevel = "2" },
                    new ChargePoint { ChargePointId = "Point2", FloorLevel = "2" },
                    new ChargePoint { ChargePointId = "Point3", FloorLevel = "3", Status = ChargePointStatus.Removed },
                }
            };

            var updatedLocationModel = new LocationModel
            {
                ChargePoints = new List<ChargePointModel>
                {
                    new ChargePointModel { ChargePointId = "Point1", FloorLevel = "2" },
                    new ChargePointModel { ChargePointId = "Point2", FloorLevel = "2" },
                    new ChargePointModel { ChargePointId = "Point3", FloorLevel = "3", Status = "Removed" },
                }
            };

            // Setup mocks
            _mockLocationRepository.Setup(repo => repo.GetLocationAsync(locationId))
                .ReturnsAsync(existingLocation);

            _mockChargePointRepository.Setup(repo => repo.UpdateChargePointAsync(It.IsAny<ChargePoint>()));

            _mockChargePointRepository.Setup(repo => repo.CreateChargePointAsync(It.IsAny<ChargePoint>()));

            _mockMapper.Setup(mapper => mapper.Map<ChargePoint>(It.IsAny<ChargePointModel>()))
                .Returns((ChargePointModel chargePointRequest) => new ChargePoint
                {
                    ChargePointId = chargePointRequest.ChargePointId,
                    Status = Enum.Parse<ChargePointStatus>(chargePointRequest.Status!, true),
                    FloorLevel = chargePointRequest.FloorLevel,
                    LastUpdated = chargePointRequest.LastUpdated
                });

            _mockMapper.Setup(mapper => mapper.Map<LocationModel>(It.IsAny<Location>()))
                .Returns(updatedLocationModel);

            // Act
            
            var result = await _service.UpsertChargePointsAsync(locationId, chargePointRequest);

            // Assert
            _mockLocationRepository.Verify(repo => repo.GetLocationAsync(locationId), Times.Exactly(2));
            _mockChargePointRepository.Verify(repo => repo.UpdateChargePointAsync(It.Is<ChargePoint>(cp => cp.ChargePointId == "Point1" && cp.FloorLevel == "2")), Times.Once);
            _mockChargePointRepository.Verify(repo => repo.CreateChargePointAsync(It.Is<ChargePoint>(cp => cp.ChargePointId == "Point2" && cp.FloorLevel == "2")), Times.Once);
            _mockChargePointRepository.Verify(repo => repo.UpdateChargePointAsync(It.Is<ChargePoint>(cp => cp.ChargePointId == "Point3" && cp.FloorLevel == "3" && cp.Status == ChargePointStatus.Removed)), Times.Once);
            _mockMapper.Verify(mapper => mapper.Map<LocationModel>(It.IsAny<Location>()), Times.Once);
        }
        #endregion
    }
}
