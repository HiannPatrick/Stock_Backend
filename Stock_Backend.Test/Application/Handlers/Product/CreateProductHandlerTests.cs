using Moq;
using Stock_Backend.Application;
using Stock_Backend.Domain;
using Stock_Backend.Infrastructure;

namespace Stock_Backend.Test.Application
{
    public class CreateProductCommandTests
    {
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly CreateProductHandler _handler;

        public CreateProductCommandTests()
        {
            _productRepositoryMock = new Mock<IProductRepository>();

            _handler = new CreateProductHandler( _productRepositoryMock.Object );
        }

        [Fact]
        public async Task Should_Create_Product_Successfully()
        {
            var command = new CreateProductCommand("Test Product 1", "PN11", 5.2m);

            var productMockSetup = _productRepositoryMock.Setup( o => o.Create( It.IsAny<ProductDto>() ) );

            productMockSetup.ReturnsAsync( true );

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.True( result.Success );

            _productRepositoryMock.Verify( o => o.Create( It.IsAny<ProductDto>() ), Times.Once );
        }

        [Fact]
        public async Task Should_Return_Error_If_Product_Creation_Fails()
        {
            var command = new CreateProductCommand("Test Product 1", "PN11", 5.2m);

            var productMockSetup = _productRepositoryMock.Setup( o => o.Create( It.IsAny<ProductDto>() ) );

            productMockSetup.ThrowsAsync( new Exception( "Falha ao criar o produto!" ) );

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.False( result.Success );

            Assert.Equal( "Falha ao criar o produto!", result.Message );
        }
    }
}
