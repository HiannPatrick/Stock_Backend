using FluentValidation;
using Stock_Backend.Domain;

namespace Stock_Backend.Application
{
    public class ProductMovementValidator :AbstractValidator<ProductMovementDto>
    {
        public ProductMovementValidator()
        {
            RuleFor( o => o.MovementType ).NotEmpty().WithMessage( "O tipo de movimento é um campo obrigatório." );
            RuleFor( o => o.MovementType ).Must( o => o == 'I' || o == 'O' ).WithMessage( "O tipo de movimento deve ser I para entrada e O para saída" );
            RuleFor( o => o.Quantity ).GreaterThan( 0 ).WithMessage( "A quantidade deve ser maior que zero." );
        }
    }
}
