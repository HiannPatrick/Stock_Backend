using Dapper;
using Stock_Backend.Application;
using Stock_Backend.Domain;
using System.Data;

namespace Stock_Backend.Infrastructure
{
    public class ProductMovementRepository :IProductMovementRepository
    {
        private readonly IDbConnection _dbConnection;

        public ProductMovementRepository( IDbConnection dbConnection )
        {
            _dbConnection = dbConnection;
        }
        public async Task<ReturnCommon> Update( ProductMovementDto movement )
        {
            var parameters = new DynamicParameters();

            parameters.Add( "p_productId", movement.ProductId, DbType.Int32 );
            parameters.Add( "p_quantity", movement.Quantity, DbType.Int32 );
            parameters.Add( "p_movementType", movement.MovementType, DbType.String, size: 1 );
            parameters.Add( "p_message", dbType: DbType.String, direction: ParameterDirection.Output, size: 255 );

            // Executa a stored procedure
            await _dbConnection.ExecuteAsync( "UpdateInventoryAndLogMovement", parameters, commandType: CommandType.StoredProcedure );

            var message = parameters.Get<string>("p_message");

            bool success = message.StartsWith("Sucesso");

            return success ? ReturnCommon.SuccessMessage( message ) : ReturnCommon.FailureMessage( message );
        }
        public async Task<List<DailyProductMovementResponseDto>> GetDailyProductMovement( DailyProductMovementRequestDto movement )
        {
            var query = @"SELECT
                            p.Id AS ProductId
                            ,p.Name AS ProductName
                            ,im.MovementDate
                            ,COALESCE(SUM(CASE WHEN im.MovementType = 'I' THEN im.Quantity ELSE -im.Quantity END), 0) AS DailyQuantityMoved
                            ,COALESCE(AVG(im.AverageCost), 0) AS DailyAverageCost
                         FROM
                            Product p
                         JOIN 
                            InventoryMovement im ON p.Id = im.ProductId
                         WHERE
                            DATE(im.MovementDate) = DATE(@MovementDate)
                         GROUP BY
                            p.Id
                            ,p.Name
                            ,im.MovementDate;";

            var parameters = new { MovementDate = movement.MovementDate };

            var result = await _dbConnection.QueryAsync<DailyProductMovementResponseDto>(query, parameters);

            return result.ToList();
        }
    }
}
