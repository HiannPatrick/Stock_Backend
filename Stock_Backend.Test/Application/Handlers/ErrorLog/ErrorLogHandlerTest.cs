using Dapper;
using MediatR;
using Moq;
using Stock_Backend.Application.Handlers.ErrorLog;
using System.Data;

namespace Stock_Backend.Test.Application
{
    public class ErrorLogHandlerTest
    {
        private readonly Mock<IDbConnection> _mockDbConnection;
        private readonly ErrorLogHandler<TestCommand, TestResponse> _errorLogHandler;

        public ErrorLogHandlerTest()
        {
            _mockDbConnection = new Mock<IDbConnection>();

            _errorLogHandler = new ErrorLogHandler<TestCommand, TestResponse>( _mockDbConnection.Object );
        }

        [Fact]
        public async Task Should_Log_Error_When_Exception_Occurs()
        {
            var testCommand = new TestCommand();

            var cancellationToken = new CancellationToken();

            var handler = new Mock<IRequestHandler<TestCommand, TestResponse>>();
            
            handler.Setup( o => o.Handle( It.IsAny<TestCommand>(), cancellationToken ) )
                   .ThrowsAsync( new Exception( "Teste de Exceção" ) );

            RequestHandlerDelegate<TestResponse> next = () => handler.Object.Handle(testCommand, cancellationToken);

            await Assert.ThrowsAsync<Exception>( () => _errorLogHandler.Handle( testCommand, next, cancellationToken ) );

            _mockDbConnection.Verify( o => o.ExecuteAsync( It.IsAny<CommandDefinition>() ), Times.Once );

            string insert = "INSERT INTO ErrorLog (ErrorMessage, StackTrace, CommandType, CreatedAt) VALUES (@ErrorMessage, @StackTrace, @CommandType, @CreatedAt);";

            _mockDbConnection.Verify( o => o.ExecuteAsync( insert,
                                           It.Is<DynamicParameters>( o => o.Get<string>( "ErrorMessage" ) == "Test exception" &&
                                                                          o.Get<string>( "CommandType" ) == nameof( TestCommand ) ),
                                                                          null,
                                                                          null,
                                                                          null ),
                                           Times.Once );
        }
    }
    public class TestCommand :IRequest<TestResponse> { }

    public class TestResponse { }

}
