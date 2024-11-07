using Stock_Backend.Application;
using Stock_Backend.Domain;

namespace Stock_Backend.Infrastructure
{
    public interface IProductMovementRepository
    {
        Task<ReturnCommon> Update( ProductMovementDto movement );
        Task<List<DailyProductMovementResponseDto>> GetDailyProductMovement( DailyProductMovementRequestDto movement );
    }
}
