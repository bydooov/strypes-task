using ICT.Strypes.Api.Controllers;
using ICT.Strypes.Business.Exceptions;
using ICT.Strypes.Business.Interfaces;
using ICT.Strypes.Business.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace ICT.Strypes.UnitTests.ControllerTests
{
    public class LocationsControllerTests
    {
        private readonly Mock<ILocationService> _mockLocationService;
        private readonly LocationsController _controller;

        public LocationsControllerTests()
        {
            _mockLocationService = new Mock<ILocationService>();
            _controller = new LocationsController(_mockLocationService.Object);
        }

        #region POST Location

        [Fact]
        public async Task PostLocationAsync_ShouldReturnCreated_WhenLocationIsAddedSuccessfully()
        {
            // Arrange
            var request = new LocationRequestModel
            {
                LocationId = "Location1"
            };

            var locationModel = new LocationModel
            {
                LocationId = "Location1"
            };

            _mockLocationService
                .Setup(service => service.PostLocationAsync(request))
                .ReturnsAsync(locationModel);

            // Act
            var result = await _controller.PostLocationAsync(request);

            // Assert
            var createdResult = Assert.IsType<CreatedResult>(result.Result);
            Assert.Equal(StatusCodes.Status201Created, createdResult.StatusCode);
            Assert.Equal(locationModel, createdResult.Value);
        }

        [Fact]
        public async Task PostLocationAsync_ShouldReturnConflict_WhenLocationAlreadyExists()
        {
            // Arrange
            var request = new LocationRequestModel
            {
                LocationId = "Location1"
            };

            _mockLocationService
                .Setup(service => service.PostLocationAsync(request))
                .ThrowsAsync(new ConflictException(string.Format("Location with id: {0} already exist.", request.LocationId)));

            // Act and Assert
            var exception = await Assert.ThrowsAsync<ConflictException>(() => _controller.PostLocationAsync(request));
            Assert.Equal(string.Format("Location with id: {0} already exist.", request.LocationId), exception.Message);
        }

        [Fact]
        public async Task PostLocationAsync_ShouldReturnInternalServerError_WhenUnhandledExceptionOccurs()
        {
            // Arrange
            var request = new LocationRequestModel
            {
                LocationId = "Location1"
            };

            _mockLocationService
                .Setup(service => service.PostLocationAsync(request))
                .ThrowsAsync(new Exception("Unhandled exception"));

            // Act and Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _controller.PostLocationAsync(request));
            Assert.Equal("Unhandled exception", exception.Message);
        }

        #endregion

        #region GET Location

        [Fact]
        public async Task GetLocationAsync_ShouldReturnLocation_WhenLocationIsInTheDatabase()
        {
            // Arrange
            string locationId = "Location1";

            var locationModel = new LocationModel
            {
                LocationId = locationId
            };

            _mockLocationService
                .Setup(service => service.GetLocationByIdAsync(locationId))
                .ReturnsAsync(locationModel);

            // Act
            var result = await _controller.GetLocationAsync(locationId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedLocation = Assert.IsType<LocationModel>(okResult.Value);

            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.Equal(locationId, returnedLocation.LocationId);
        }

        [Fact]
        public async Task GetLocationAsync_ShouldReturnNotFoundException_WhenLocationIsNotInTheDatabase()
        {
            string locationId = "Location1";

            _mockLocationService
                .Setup(service => service.GetLocationByIdAsync(locationId))
                .ThrowsAsync(new NotFoundException(string.Format("Location with id: {0} not found.", locationId)));

            // Act and Assert
            var exception = await Assert.ThrowsAsync<NotFoundException>(() => _controller.GetLocationAsync(locationId));
            Assert.Equal(string.Format("Location with id: {0} not found.", locationId), exception.Message);
        }

        [Fact]
        public async Task GetLocationAsync_ShouldReturnInternalServerException_WhenUnhandledExceptionOccurs()
        {
            string locationId = "Location1";

            _mockLocationService
                .Setup(service => service.GetLocationByIdAsync(locationId))
                .ThrowsAsync(new Exception("Unhandled exception"));

            // Act and Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _controller.GetLocationAsync(locationId));
            Assert.Equal("Unhandled exception", exception.Message);
        }

        #endregion

        #region PATCH Location

        [Fact]
        public async Task PatchLocationAsync_ShouldReturnLocation_WhenLocationIsSuccessfulyUpdated()
        {
            // Arrange
            string locationId = "Location1";
            string newName = "New Name";

            var patchLocationRequest = new PatchLocationRequestModel
            {
                Name = newName
            };

            var locationModel = new LocationModel
            {
                LocationId = locationId,
                Name = newName
            };

            _mockLocationService
                .Setup(service => service.PatchLocationAsync(locationId, patchLocationRequest))
                .ReturnsAsync(locationModel);

            // Act
            var result = await _controller.PatchLocationAsync(locationId, patchLocationRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedLocation = Assert.IsType<LocationModel>(okResult.Value);

            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.Equal(newName, returnedLocation.Name);
        }

        [Fact]
        public async Task PatchLocationAsync_ShouldReturnNotFoundException_WhenLocationIsNotFound()
        {
            string locationId = "Location1";

            _mockLocationService
                .Setup(service => service.PatchLocationAsync(locationId, It.IsAny<PatchLocationRequestModel>()))
                .ThrowsAsync(new NotFoundException(string.Format("Location with id: {0} not found.", locationId)));

            // Act and Assert
            var exception = await Assert.ThrowsAsync<NotFoundException>(() => _controller.PatchLocationAsync(locationId, It.IsAny<PatchLocationRequestModel>()));
            Assert.Equal(string.Format("Location with id: {0} not found.", locationId), exception.Message);
        }

        [Fact]
        public async Task PatchLocationAsync_ShouldReturnInternalServerException_WhenUnhandledExceptionOccurs()
        {
            string locationId = "Location1";

            _mockLocationService
                .Setup(service => service.PatchLocationAsync(locationId, It.IsAny<PatchLocationRequestModel>()))
                .ThrowsAsync(new Exception("Unhandled exception"));

            // Act and Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _controller.PatchLocationAsync(locationId, It.IsAny<PatchLocationRequestModel>()));
            Assert.Equal("Unhandled exception", exception.Message);
        }

        #endregion

        #region PUT Location

        [Fact]
        public async Task PutLocationAsync_ShouldReturnLocation_WhenChargePointsAreSuccessfulyUpserted()
        {
            // Arrange
            string locationId = "Location1";

            var chargePointRequestModel = new ChargePointRequestModel
            {
                ChargePoints =
                [
                    new() {ChargePointId = "ChargePoint1"}
                ]
            };

            var locationModel = new LocationModel
            {
                LocationId = locationId,
                ChargePoints =
                [
                    new() {ChargePointId = "ChargePoint1"}
                ]
            };

            _mockLocationService
                .Setup(service => service.UpsertChargePointsAsync(locationId, chargePointRequestModel))
                .ReturnsAsync(locationModel);

            // Act
            var result = await _controller.UpsertChargePointsAsync(locationId, chargePointRequestModel);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedLocation = Assert.IsType<LocationModel>(okResult.Value);

            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.Equal(locationModel.ChargePoints.First().ChargePointId, returnedLocation.ChargePoints!.First().ChargePointId);
        }

        [Fact]
        public async Task PutLocationAsync_ShouldReturnNotFoundException_WhenLocationIsNotFound()
        {
            string locationId = "Location1";

            _mockLocationService
                .Setup(service => service.UpsertChargePointsAsync(locationId, It.IsAny<ChargePointRequestModel>()))
                .ThrowsAsync(new NotFoundException(string.Format("Location with id: {0} not found.", locationId)));

            // Act and Assert
            var exception = await Assert.ThrowsAsync<NotFoundException>(() => _controller.UpsertChargePointsAsync(locationId, It.IsAny<ChargePointRequestModel>()));
            Assert.Equal(string.Format("Location with id: {0} not found.", locationId), exception.Message);
        }

        [Fact]
        public async Task PutLocationAsync_ShouldReturnInternalServerException_WhenUnhandledExceptionOccurs()
        {
            string locationId = "Location1";

            _mockLocationService
                .Setup(service => service.UpsertChargePointsAsync(locationId, It.IsAny<ChargePointRequestModel>()))
                .ThrowsAsync(new Exception("Unhandled exception"));

            // Act and Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _controller.UpsertChargePointsAsync(locationId, It.IsAny<ChargePointRequestModel>()));
            Assert.Equal("Unhandled exception", exception.Message);
        }

        #endregion
    }
}
