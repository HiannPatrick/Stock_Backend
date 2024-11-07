using MediatR;
using Stock_Backend.Domain;

namespace Stock_Backend.Application
{
    public record GetAllProductsCommand :IRequest<List<ProductDto>>;
}
