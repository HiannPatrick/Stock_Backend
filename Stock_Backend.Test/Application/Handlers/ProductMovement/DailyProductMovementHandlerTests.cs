using Moq;
using Stock_Backend.Application;
using Stock_Backend.Domain;
using Stock_Backend.Infrastructure;

namespace Stock_Backend.Test.Application.Handlers.ProductMovement
{
    public class DailyProductMovementHandlerTests
    {
        private readonly Mock<IProductMovementRepository> _productMovementRepositoryMock;
        private readonly DailyProductMovementHandler _handler;

        public DailyProductMovementHandlerTests()
        {
            _productMovementRepositoryMock = new Mock<IProductMovementRepository>();
            _handler = new DailyProductMovementHandler( _productMovementRepositoryMock.Object );
        }

        [Fact]
        public async Task ReturnsEmptyList_WhenNoDataExists()
        {
            var movementRequest = new DailyProductMovementRequestDto{ MovementDate = new DateTime(2024, 11, 05) };

            var expectedMovements = new List<DailyProductMovementResponseDto>();

            var productMockSetup = _productMovementRepositoryMock.Setup( o => o.GetDailyProductMovement( movementRequest ) );

            productMockSetup.ReturnsAsync( expectedMovements );

            var command = new DailyProductMovementRequestCommand(movementRequest.MovementDate);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.NotNull( result );
            Assert.Empty( result );
        }
    }
}
