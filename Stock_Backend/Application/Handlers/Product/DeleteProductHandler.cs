using MediatR;
using Stock_Backend.Infrastructure;

namespace Stock_Backend.Application
{
    public class DeleteProductHandler :IRequestHandler<DeleteProductCommand, ReturnCommon>
    {
        private readonly IProductRepository _productRepository;

        public DeleteProductHandler( IProductRepository productRepository )
        {
            _productRepository = productRepository;
        }

        public async Task<ReturnCommon> Handle( DeleteProductCommand request, CancellationToken cancellationToken )
        {
            try
            {
                var produto = await _productRepository.GetProductById(request.Id);

                if( produto == null )
                {
                    return ReturnCommon.FailureMessage( "Produto não localizado!" );
                }

                await _productRepository.Delete( produto.Id );

                return ReturnCommon.SuccessMessage( "Produto excluído com sucesso." );
            }
            catch( Exception )
            {
                return ReturnCommon.FailureMessage( "Erro ao excluir o produto!" );
            }
        }
    }
}
