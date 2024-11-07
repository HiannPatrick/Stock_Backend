using MediatR;
using Stock_Backend.Domain;
using Stock_Backend.Infrastructure;

namespace Stock_Backend.Application
{
    public class ProductMovementHandler :IRequestHandler<ProductMovementCommand, ReturnCommon>
    {
        private readonly IProductMovementRepository _productMovementRepository;
        private readonly ProductMovementValidator _productMovementValidator;

        public ProductMovementHandler( IProductMovementRepository productMovementRepository )
        {
            _productMovementRepository = productMovementRepository;
            _productMovementValidator = new ProductMovementValidator();
        }

        public async Task<ReturnCommon> Handle( ProductMovementCommand request, CancellationToken cancellationToken )
        {
            var productMovement = new ProductMovementDto
            {
                Quantity = request.Quantity,
                Message = request.Message,
                MovementType = request.MovementType,
                ProductId = request.ProductId
            };

            var validation = _productMovementValidator.Validate( productMovement );

            if( !validation.IsValid )
            {
                string error = validation.GetErrorMessage();

                return ReturnCommon.FailureMessage( error );
            }

            var returnCommom = await _productMovementRepository.Update( productMovement );

            return returnCommom;
        }
    }
}
