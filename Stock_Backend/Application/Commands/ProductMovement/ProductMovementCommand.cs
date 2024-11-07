using MediatR;

namespace Stock_Backend.Application
{
    public record ProductMovementCommand( int ProductId, int Quantity, char MovementType, string Message ) :IRequest<ReturnCommon>;
}