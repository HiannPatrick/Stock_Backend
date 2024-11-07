using FluentValidation;
using Stock_Backend.Domain;

namespace Stock_Backend.Application
{
    public class ProductValidator :AbstractValidator<ProductDto>
    {
        public ProductValidator()
        {
            RuleFor( o => o.Name ).NotEmpty().WithMessage( "Nome é um campo obrigatório." );
            RuleFor( o => o.PartNumber ).NotEmpty().WithMessage( "PartNumber é um campo obrigatório." );
            RuleFor( o => o.AverageCostPrice ).GreaterThan( 0 ).WithMessage( "O preço de custo deve ser maior que zero." );
        }
    }
}
