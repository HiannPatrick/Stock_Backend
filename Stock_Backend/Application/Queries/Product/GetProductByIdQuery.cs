using MediatR;
using Stock_Backend.Domain;

namespace Stock_Backend.Application
{
    public record GetProductByIdQuery( int Id ) :IRequest<ProductDto>;
}
