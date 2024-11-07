using MediatR;
using Stock_Backend.Domain;

namespace Stock_Backend.Application
{
    public record DailyProductMovementRequestCommand( DateTime MovementDate ) :IRequest<List<DailyProductMovementResponseDto>>;
}
