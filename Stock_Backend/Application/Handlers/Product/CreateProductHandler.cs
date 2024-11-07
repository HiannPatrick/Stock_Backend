using MediatR;
using Stock_Backend.Domain;
using Stock_Backend.Infrastructure;

namespace Stock_Backend.Application
{
    public class CreateProductHandler :IRequestHandler<CreateProductCommand, ReturnCommon>
    {
        private readonly IProductRepository _productRepository;
        private readonly ProductValidator _productValidator;
        public CreateProductHandler( IProductRepository productRepository )
        {
            _productRepository = productRepository;
            _productValidator = new ProductValidator();
        }

        public async Task<ReturnCommon> Handle( CreateProductCommand request, CancellationToken cancellationToken )
        {
            try
            {
                var product = new ProductDto
                {
                    Name = request.Name,
                    PartNumber = request.PartNumber,
                    AverageCostPrice = request.AverageCostPrice
                };

                var validation = _productValidator.Validate( product );

                if( !validation.IsValid )
                {
                    string error = validation.GetErrorMessage();

                    return ReturnCommon.FailureMessage( error );
                }

                var hadSuccess = await _productRepository.Create(product);

                return hadSuccess
                       ? ReturnCommon.SuccessMessage( "Produto criado com sucesso!" )
                       : ReturnCommon.FailureMessage( "Falha ao criar o produto!" );
            }
            catch( Exception )
            {
                return ReturnCommon.FailureMessage( "Falha ao criar o produto!" );
            }
        }
    }
}
