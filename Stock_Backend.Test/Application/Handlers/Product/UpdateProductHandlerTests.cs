using Moq;
using Stock_Backend.Application;
using Stock_Backend.Domain;
using Stock_Backend.Infrastructure;

namespace Stock_Backend.Test.Application
{
    public class UpdateProductHandlerTests
    {
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly UpdateProductHandler _handler;

        public UpdateProductHandlerTests()
        {
            _productRepositoryMock = new Mock<IProductRepository>();
            _handler = new UpdateProductHandler( _productRepositoryMock.Object );
        }

        [Fact]
        public async Task Should_Update_Product_Successfully()
        {
            var command = new UpdateProductCommand(1, "Product 20", "Product 20", 20.0m);

            var productMockSetup = _productRepositoryMock.Setup( o => o.GetProductById( It.IsAny<int>() ) );

            productMockSetup.ReturnsAsync( new ProductDto() );

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.True( result.Success );

            _productRepositoryMock.Verify( o => o.Update( It.IsAny<ProductDto>() ), Times.Once );
        }

        [Fact]
        public async Task Should_Return_Error_If_Product_Not_Found()
        {
            var command = new UpdateProductCommand(1, "Product 20", "PN20", 20.0m);

            var productMockSetup = _productRepositoryMock.Setup( o => o.GetProductById( It.IsAny<int>() ) );

            productMockSetup.ReturnsAsync( (ProductDto)null );

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.False( result.Success );

            Assert.Equal( "Produto não localizado!", result.Message );
        }
    }
}
