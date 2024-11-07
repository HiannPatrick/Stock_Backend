using Moq;
using Stock_Backend.Application;
using Stock_Backend.Domain;
using Stock_Backend.Infrastructure;

namespace Stock_Backend.Test.Application.Handlers.ProductMovement
{
    public class ProductMovementHandlerTests
    {
        private readonly Mock<IProductMovementRepository> _productMovementRepositoryMock;
        private readonly ProductMovementHandler _handler;

        public ProductMovementHandlerTests()
        {
            _productMovementRepositoryMock = new Mock<IProductMovementRepository>();
            _handler = new ProductMovementHandler( _productMovementRepositoryMock.Object );
        }

        [Fact]
        public async Task ShouldReturnSuccess_WhenRepositoryUpdatesSuccessfully()
        {
            var command = new ProductMovementCommand(1,10, 'I', "Movimento de entrada" );

            var expectedResponse = ReturnCommon.SuccessMessage( "Movimento registrado com sucesso" );

            var productMockSetup = _productMovementRepositoryMock
                                    .Setup( o => o.Update( It.IsAny<ProductMovementDto>() ) );

            productMockSetup.ReturnsAsync( expectedResponse );

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.NotNull( result );
            Assert.True( result.Success );
            Assert.Equal( "Movimento registrado com sucesso", result.Message );

            _productMovementRepositoryMock.Verify( o => o.Update( It.IsAny<ProductMovementDto>() ), Times.Once );
        }

        [Fact]
        public async Task ShouldReturnFailure_WhenRepositoryReturnsFailure()
        {
            // Arrange
            var command = new ProductMovementCommand( 2, 5, 'O', "Movimento de saída" );

            var expectedResponse = ReturnCommon.FailureMessage("Erro ao registrar o movimento");

            var productMockSetup = _productMovementRepositoryMock
                .Setup( repo => repo.Update( It.IsAny<ProductMovementDto>() ) );

            productMockSetup.ReturnsAsync( expectedResponse );

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.NotNull( result );
            Assert.False( result.Success );
            Assert.Equal( "Erro ao registrar o movimento", result.Message );

            _productMovementRepositoryMock.Verify( repo => repo.Update( It.IsAny<ProductMovementDto>() ), Times.Once );
        }

        [Fact]
        public async Task ShouldCallRepositoryWithCorrectProductMovementDto()
        {
            var command = new ProductMovementCommand(3, 20, 'O', "Movimento de saída" );

            var expectedResponse = ReturnCommon.SuccessMessage("Sucesso");

            var productMockSetup = _productMovementRepositoryMock
                .Setup( o => o.Update( It.IsAny<ProductMovementDto>() ) );

            productMockSetup.ReturnsAsync( expectedResponse );

            await _handler.Handle( command, CancellationToken.None );

            _productMovementRepositoryMock.Verify( o => o.Update( It.Is<ProductMovementDto>( dto =>
                dto.Quantity == command.Quantity &&
                dto.Message == command.Message &&
                dto.MovementType == command.MovementType &&
                dto.ProductId == command.ProductId
            ) ), Times.Once );
        }
    }
}
