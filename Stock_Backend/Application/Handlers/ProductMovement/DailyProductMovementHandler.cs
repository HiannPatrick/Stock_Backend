using MediatR;
using Stock_Backend.Domain;
using Stock_Backend.Infrastructure;

namespace Stock_Backend.Application
{
    public class DailyProductMovementHandler :IRequestHandler<DailyProductMovementRequestCommand, List<DailyProductMovementResponseDto>>
    {
        private readonly IProductMovementRepository _productMovementRepository;

        public DailyProductMovementHandler( IProductMovementRepository productMovementRepository )
        {
            _productMovementRepository = productMovementRepository;
        }

        public async Task<List<DailyProductMovementResponseDto>> Handle( DailyProductMovementRequestCommand request, CancellationToken cancellationToken )
        {
            var dailyProductMovementRequest = new DailyProductMovementRequestDto
            {
                MovementDate = request.MovementDate
            };

            var dailyProductMovementResponse = await _productMovementRepository.GetDailyProductMovement( dailyProductMovementRequest );

            return dailyProductMovementResponse ?? new List<DailyProductMovementResponseDto>();
        }
    }
}
