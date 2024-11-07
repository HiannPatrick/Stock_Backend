using MediatR;
using Stock_Backend.Domain;
using Stock_Backend.Infrastructure;

namespace Stock_Backend.Application
{
    public class GetAllProductsHandler :IRequestHandler<GetAllProductsCommand, List<ProductDto>>
    {
        private readonly IProductRepository _productRepository;

        public GetAllProductsHandler( IProductRepository productRepository )
        {
            _productRepository = productRepository;
        }

        public async Task<List<ProductDto>> Handle( GetAllProductsCommand request, CancellationToken cancellationToken )
        {
            var products = await _productRepository.GetAllProducts();

            return products;
        }
    }
}
