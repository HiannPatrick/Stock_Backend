using Moq;
using Stock_Backend.Application;
using Stock_Backend.Domain;
using Stock_Backend.Infrastructure;

namespace Stock_Backend.Test.Application
{
    public class GetProductByIdHandlerTests
    {
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly GetProductByIdHandler _handler;

        public GetProductByIdHandlerTests()
        {
            _productRepositoryMock = new Mock<IProductRepository>();
            _handler = new GetProductByIdHandler( _productRepositoryMock.Object );
        }

        [Fact]
        public async Task Should_Return_Product_If_Product_Found()
        {
            var product = new ProductDto { Id = 1, Name = "Product 1", PartNumber = "PN123", AverageCostPrice = 5.3m };

            var command = new GetProductByIdCommand(1);

            var productMockSetup = _productRepositoryMock.Setup( o => o.GetProductById( 1 ) );

            productMockSetup.ReturnsAsync( product );

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.NotNull( result );
            Assert.Equal( "Product 1", result.Name );
            Assert.Equal( "PN123", result.PartNumber );
            Assert.Equal( 5.3m, result.AverageCostPrice );
        }

        [Fact]
        public async Task Should_Return_Null_If_Product_Not_Found()
        {
            var command = new GetProductByIdCommand(1);

            var productMockSetup = _productRepositoryMock.Setup( o => o.GetProductById( 1 ) );

            productMockSetup.ReturnsAsync( (ProductDto)null );

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.Null( result );
        }
    }
}
