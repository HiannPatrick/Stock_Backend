using MediatR;
using Stock_Backend.Domain;

namespace Stock_Backend.Application
{
    public record GetProductByIdCommand( int Id ) :IRequest<ProductDto>;
}
