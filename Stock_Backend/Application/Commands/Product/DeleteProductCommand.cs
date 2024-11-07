using MediatR;

namespace Stock_Backend.Application
{
    public record DeleteProductCommand( int Id ) :IRequest<ReturnCommon>;
}
