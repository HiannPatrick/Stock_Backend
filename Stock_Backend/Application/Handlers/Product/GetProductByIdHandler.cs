using MediatR;
using Stock_Backend.Domain;
using Stock_Backend.Infrastructure;

namespace Stock_Backend.Application
{
    public class GetProductByIdHandler :IRequestHandler<GetProductByIdCommand, ProductDto>
    {
        private readonly IProductRepository _productRepository;

        public GetProductByIdHandler( IProductRepository productRepository )
        {
            _productRepository = productRepository;
        }

        public async Task<ProductDto> Handle( GetProductByIdCommand request, CancellationToken cancellationToken )
        {
            var product = await _productRepository.GetProductById(request.Id);

            return product;
        }
    }
}
