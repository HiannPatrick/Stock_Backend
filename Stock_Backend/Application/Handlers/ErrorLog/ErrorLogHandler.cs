using Dapper;
using MediatR;
using System.Data;

namespace Stock_Backend.Application.Handlers.ErrorLog
{
    public class ErrorLogHandler<TRequest, TResponse> :IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IDbConnection _dbConnection;

        public ErrorLogHandler( IDbConnection dbConnection )
        {
            _dbConnection = dbConnection;
        }

        public async Task<TResponse> Handle( TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken )
        {
            try
            {
                return await next();
            }
            catch( Exception ex )
            {
                registerException( ex );

                throw;
            }
        }

        private void registerException( Exception ex )
        {
            try
            {
                var commandType = typeof(TRequest).Name;

                var parametros = new DynamicParameters();

                parametros.Add( "ErrorMessage", ex.Message );
                parametros.Add( "StackTrace", ex.StackTrace );
                parametros.Add( "CommandType", commandType );
                parametros.Add( "CreatedAt", DateTime.Now );

                _dbConnection.ExecuteAsync(
                    @"INSERT INTO ErrorLog 
                      (ErrorMessage, StackTrace, CommandType, CreatedAt) 
                      VALUES (@ErrorMessage, @StackTrace, @CommandType, @CreatedAt);",
                    parametros
                );
            }
            finally { }
        }
    }
}
