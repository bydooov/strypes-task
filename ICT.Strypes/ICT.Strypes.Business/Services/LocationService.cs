using AutoMapper;
using ICT.Strypes.Business.Exceptions;
using ICT.Strypes.Business.Interfaces;
using ICT.Strypes.Business.Models;
using ICT.Strypes.Business.Resources;
using ICT.Strypes.Domain.Entities;
using ICT.Strypes.Infrastructure.Interfaces;

namespace ICT.Strypes.Business.Services
{
    public class LocationService : ILocationService
    {
        private readonly ILocationRepository _locationRepository;
        private readonly IChargePointRepository _chargePointRepository;
        private readonly IMapper _mapper;

        public LocationService(ILocationRepository locationRepository, IMapper mapper, IChargePointRepository chargePointRepository)
        {
            _locationRepository = locationRepository;
            _mapper = mapper;
            _chargePointRepository = chargePointRepository;
        }

        public async Task<LocationModel> GetLocationByIdAsync(string locationId)
        {
            var location = await GetLocationAsync(locationId).ConfigureAwait(false)
                ?? throw new NotFoundException(string.Format(ErrorMessages.LocationNotFoundErrorMessage, locationId));

            return _mapper.Map<LocationModel>(location);
        }

        public async Task<LocationModel> PatchLocationAsync(string locationId, PatchLocationRequestModel request)
        {
            var location = await GetLocationAsync(locationId).ConfigureAwait(false)
                ?? throw new NotFoundException(string.Format(ErrorMessages.LocationNotFoundErrorMessage, locationId));

            _mapper.Map(request, location);

            var updatedLocation = await _locationRepository.UpdateLocationAsync(location!);

            return _mapper.Map<LocationModel>(updatedLocation);
        }

        public async Task<LocationModel> PostLocationAsync(LocationRequestModel request)
        {
            var existingLocation = await GetLocationAsync(request.LocationId!).ConfigureAwait(false);

            if (existingLocation is not null)
            {
                throw new ConflictException(string.Format(ErrorMessages.LocationDuplicatedErrorMessage, request.LocationId));
            }

            var location = _mapper.Map<Location>(request);

            var createdLocation = await _locationRepository.CreateLocationAsync(location).ConfigureAwait(false);

            return _mapper.Map<LocationModel>(createdLocation);
        }

        public async Task<LocationModel> UpsertChargePointsAsync(string locationId, ChargePointRequestModel request)
        {
            var location = await GetLocationAsync(locationId).ConfigureAwait(false)
                ?? throw new NotFoundException(string.Format(ErrorMessages.LocationNotFoundErrorMessage, locationId));

            await RemoveNotRequestedChargePointsAsync(location.ChargePoints!, request.ChargePoints!.Select(cp =>
                cp.ChargePointId)).ConfigureAwait(false);

            foreach (var requestedChargePoint in request.ChargePoints!)
            {
                var existingLocationChargePoint = location.ChargePoints!
                    .Where(cp => cp.ChargePointId == requestedChargePoint.ChargePointId)
                    .FirstOrDefault();

                if (existingLocationChargePoint is not null)
                {
                    if (existingLocationChargePoint.Status == ChargePointStatus.Removed)
                    {
                        throw new BadRequestException(string.Format(ErrorMessages.ChargePointRemovedErrorMessage, existingLocationChargePoint.ChargePointId));
                    }

                    existingLocationChargePoint.LastUpdated = requestedChargePoint.LastUpdated;
                    existingLocationChargePoint.Status = Enum.Parse<ChargePointStatus>(requestedChargePoint.Status!, true);
                    existingLocationChargePoint.FloorLevel = requestedChargePoint.FloorLevel;

                    await _chargePointRepository.UpdateChargePointAsync(existingLocationChargePoint).ConfigureAwait(false);
                }
                else
                {
                    if (await _chargePointRepository.DoChargePointExistAsync(requestedChargePoint.ChargePointId!).ConfigureAwait(false))
                    {
                        throw new BadRequestException(string.Format(ErrorMessages.ChargePointDuplicatedErrorMessage, requestedChargePoint.ChargePointId));
                    }
                    var chargePoint = _mapper.Map<ChargePoint>(requestedChargePoint);
                    chargePoint.LocationId = locationId;

                    await _chargePointRepository.CreateChargePointAsync(chargePoint).ConfigureAwait(false);
                }
            }

            var updatedLocation = await _locationRepository.GetLocationAsync(locationId).ConfigureAwait(false);

            return _mapper.Map<LocationModel>(updatedLocation);
        }

        private async Task RemoveNotRequestedChargePointsAsync(IEnumerable<ChargePoint> existingChargePoints, IEnumerable<string?> requestedChargePointsIds)
        {
            var notRequestedChargePoints = existingChargePoints.Where(chargePoint =>
                !requestedChargePointsIds.Contains(chargePoint.ChargePointId));

            if (notRequestedChargePoints.Any() && notRequestedChargePoints.Any(cp => cp.Status != ChargePointStatus.Removed))
            {
                foreach (var notRequestedChargePoint in notRequestedChargePoints.Where(cp => cp.Status != ChargePointStatus.Removed))
                {
                    notRequestedChargePoint.Status = ChargePointStatus.Removed;

                    await _chargePointRepository.UpdateChargePointAsync(notRequestedChargePoint).ConfigureAwait(false);
                }
            }
        }

        private async Task<Location> GetLocationAsync(string locationId)
        {
            return await _locationRepository.GetLocationAsync(locationId).ConfigureAwait(false);
        }
    }
}
