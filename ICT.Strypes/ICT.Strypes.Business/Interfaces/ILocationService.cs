using ICT.Strypes.Business.Models;

namespace ICT.Strypes.Business.Interfaces
{
    public interface ILocationService
    {
        Task<LocationModel> PostLocationAsync(LocationRequestModel request);
        Task<LocationModel> GetLocationByIdAsync(string locationId);
        Task<LocationModel> PatchLocationAsync(string locationId, PatchLocationRequestModel request);
        Task<LocationModel> UpsertChargePointsAsync(string locationId, ChargePointRequestModel request);
    }
}
