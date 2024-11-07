using Moq;
using Stock_Backend.Application;
using Stock_Backend.Domain;
using Stock_Backend.Infrastructure;

namespace Stock_Backend.Test.Application
{
    public class DeleteProductHandlerTests
    {
        public class DeleteProductCommandHandlerTests
        {
            private readonly Mock<IProductRepository> _productRepositoryMock;
            private readonly DeleteProductHandler _handler;

            public DeleteProductCommandHandlerTests()
            {
                _productRepositoryMock = new Mock<IProductRepository>();
                _handler = new DeleteProductHandler( _productRepositoryMock.Object );
            }

            [Fact]
            public async Task Should_Delete_Product_Successfully()
            {
                var command = new DeleteProductCommand(1);

                var productMockSetup = _productRepositoryMock.Setup( o => o.GetProductById( It.IsAny<int>() ) );

                productMockSetup.ReturnsAsync( new ProductDto() );

                var result = await _handler.Handle(command, CancellationToken.None);

                Assert.True( result.Success );

                _productRepositoryMock.Verify( o => o.Delete( It.IsAny<int>() ), Times.Once );
            }

            [Fact]
            public async Task Should_Return_Error_If_Product_Not_Found()
            {
                var command = new DeleteProductCommand(1);

                var productMockSetup = _productRepositoryMock.Setup( o => o.GetProductById( It.IsAny<int>() ) );

                productMockSetup.ReturnsAsync( (ProductDto)null );

                var result = await _handler.Handle(command, CancellationToken.None);

                Assert.False( result.Success );

                Assert.Equal( "Produto não localizado!", result.Message );
            }
        }

    }
}
