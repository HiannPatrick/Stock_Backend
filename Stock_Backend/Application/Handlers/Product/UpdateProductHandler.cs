using MediatR;
using Stock_Backend.Infrastructure;

namespace Stock_Backend.Application
{
    public class UpdateProductHandler :IRequestHandler<UpdateProductCommand, ReturnCommon>
    {
        private readonly IProductRepository _productRepository;
        private readonly ProductValidator _productValidator;

        public UpdateProductHandler( IProductRepository productRepository )
        {
            _productRepository = productRepository;
            _productValidator = new ProductValidator();
        }

        public async Task<ReturnCommon> Handle( UpdateProductCommand request, CancellationToken cancellationToken )
        {
            try
            {
                var product = await _productRepository.GetProductById(request.Id);

                if( product == null )
                {
                    return ReturnCommon.FailureMessage( "Produto não localizado!" );
                }

                product.Name = request.Name;
                product.PartNumber = request.PartNumber;
                product.AverageCostPrice = request.AverageCostPrice;

                var validation = _productValidator.Validate( product );

                if( !validation.IsValid )
                {
                    string error = validation.GetErrorMessage();

                    return ReturnCommon.FailureMessage( error );
                }

                await _productRepository.Update( product );

                return ReturnCommon.SuccessMessage( "Produto atualizado com sucesso." );
            }
            catch( Exception )
            {
                return ReturnCommon.FailureMessage( "Erro ao atualizar o produto!" );
            }
        }
    }
}
