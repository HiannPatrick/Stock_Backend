using Moq;
using Stock_Backend.Application;
using Stock_Backend.Domain;
using Stock_Backend.Infrastructure;

namespace Stock_Backend.Test.Application.Handlers.Product
{
    public class GetAllProductsHandlerTests
    {
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly GetAllProductsHandler _handler;

        public GetAllProductsHandlerTests()
        {
            _productRepositoryMock = new Mock<IProductRepository>();
            _handler = new GetAllProductsHandler( _productRepositoryMock.Object );
        }

        [Fact]
        public async Task Handle_ShouldReturnListOfProducts_WhenProductsExist()
        {
            var products = new List<ProductDto>
            {
                new ProductDto { Id = 1, Name = "Product 1", PartNumber = "PN1", AverageCostPrice = 100.0m },
                new ProductDto { Id = 2, Name = "Product 2", PartNumber = "PN2", AverageCostPrice = 200.0m }
            };

            _productRepositoryMock.Setup( repo => repo.GetAllProducts() ).ReturnsAsync( products );

            var command = new GetAllProductsCommand();

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.NotNull( result );
            Assert.Equal( 2, result.Count );
            Assert.Equal( "Product 1", result[ 0 ].Name );
            Assert.Equal( "Product 2", result[ 1 ].Name );

            _productRepositoryMock.Verify( repo => repo.GetAllProducts(), Times.Once );
        }

        [Fact]
        public async Task Handle_ShouldReturnEmptyList_WhenNoProductsExist()
        {
            _productRepositoryMock.Setup( repo => repo.GetAllProducts() ).ReturnsAsync( new List<ProductDto>() );

            var command = new GetAllProductsCommand();

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.NotNull( result );
            Assert.Empty( result );

            _productRepositoryMock.Verify( repo => repo.GetAllProducts(), Times.Once );
        }
    }
}