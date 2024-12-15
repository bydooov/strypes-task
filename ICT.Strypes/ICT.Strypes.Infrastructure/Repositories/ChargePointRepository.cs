using ICT.Strypes.Domain.Entities;
using ICT.Strypes.Infrastructure.Data;
using ICT.Strypes.Infrastructure.Exceptions;
using ICT.Strypes.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ICT.Strypes.Infrastructure.Repositories
{
    public class ChargePointRepository : IChargePointRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ChargePointRepository> _logger;

        public ChargePointRepository(ApplicationDbContext context, ILogger<ChargePointRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task CreateChargePointAsync(ChargePoint chargePoint)
        {
            try
            {
                var entry = await _context.ChargePoints.AddAsync(chargePoint);

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                throw new DatabaseException(ex.Message);
            }
        }

        public async Task<bool> DoChargePointExistAsync(string chargePointId)
        {
            try
            {
                return await _context.ChargePoints
                    .AnyAsync(cp => cp.ChargePointId == chargePointId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                throw new DatabaseException(ex.Message);
            }
        }

        public async Task UpdateChargePointAsync(ChargePoint chargePoint)
        {
            try
            {
                _context.ChargePoints.Update(chargePoint);

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                throw new DatabaseException(ex.Message);
            }
        }
    }
}
