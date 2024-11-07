using Dapper;
using Stock_Backend.Domain;
using System.Data;

namespace Stock_Backend.Infrastructure
{
    public class ProductRepository :IProductRepository
    {
        private readonly IDbConnection _dbConnection;
        public ProductRepository( IDbConnection dbConnection )
        {
            _dbConnection = dbConnection;
        }
        public async Task<bool> Create( ProductDto product )
        {
            var query = @"INSERT INTO Product 
                          (Name, PartNumber, AverageCostPrice) 
                          VALUES (@Name, @PartNumber, @AverageCostPrice);";

            var result = await _dbConnection.ExecuteAsync(query, product);

            return result > 0;
        }
        public async Task<bool> Update( ProductDto product )
        {
            var query = @"UPDATE Product
                          SET Name = @Name
                              ,PartNumber = @PartNumber
                              ,AverageCostPrice = @AverageCostPrice
                          WHERE Id = @Id;";

            var result = await _dbConnection.ExecuteAsync(query, product);

            return result > 0;
        }
        public async Task<bool> Delete( int id )
        {
            var query = "DELETE FROM Product WHERE Id = @Id;";

            var result = await _dbConnection.ExecuteAsync(query, new { Id = id } );

            return result > 0;
        }
        public async Task<List<ProductDto>> GetAllProducts()
        {
            var query = "SELECT * FROM Product;";

            var result = await _dbConnection.QueryAsync<ProductDto>(query);

            return result.ToList();
        }
        public async Task<ProductDto> GetProductById( int id )
        {
            var query = "SELECT * FROM Product WHERE Id = @Id;";

            var result = await _dbConnection.QueryFirstOrDefaultAsync<ProductDto>(query, new{ Id = id} );

            return result;
        }
    }
}
