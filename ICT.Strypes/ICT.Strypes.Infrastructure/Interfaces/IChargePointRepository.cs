using ICT.Strypes.Domain.Entities;

namespace ICT.Strypes.Infrastructure.Interfaces
{
    public interface IChargePointRepository
    {
        Task CreateChargePointAsync(ChargePoint chargePoint);
        Task UpdateChargePointAsync(ChargePoint chargePoint);
        Task<bool> IsChargePointExistAsync(string chargePointId);
    }
}
