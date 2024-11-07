using Stock_Backend.Domain;

namespace Stock_Backend.Infrastructure
{
    public interface IProductRepository
    {
        Task<bool> Create( ProductDto product );
        Task<bool> Update( ProductDto product );
        Task<bool> Delete( int id );
        Task<ProductDto> GetProductById( int id );
        Task<List<ProductDto>> GetAllProducts();
    }
}
