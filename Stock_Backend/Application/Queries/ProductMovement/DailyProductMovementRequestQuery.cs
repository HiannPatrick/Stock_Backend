using MediatR;
using Stock_Backend.Domain;

namespace Stock_Backend.Application
{
    public record DailyProductMovementRequestQuery( DateTime MovementDate ) :IRequest<List<DailyProductMovementResponseDto>>;
}
