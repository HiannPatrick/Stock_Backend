using FluentValidation.TestHelper;
using Stock_Backend.Application;
using Stock_Backend.Domain;

namespace Stock_Backend.Test.Application.Validators.ProductMovement
{
    public class ProductMovementValidatorTests
    {
        private readonly ProductMovementValidator _validator;

        public ProductMovementValidatorTests()
        {
            _validator = new ProductMovementValidator();
        }

        [Fact]
        public void Should_Have_Error_When_MovementType_Is_Empty()
        {
            var dto = new ProductMovementDto { MovementType = '\0', Quantity = 10 }; // Movimento vazio

            var result = _validator.TestValidate(dto);

            result.ShouldHaveValidationErrorFor( o => o.MovementType )
                  .WithErrorMessage( "O tipo de movimento é um campo obrigatório." );
        }

        [Fact]
        public void Should_Have_Error_When_MovementType_Is_Invalid()
        {
            var dto = new ProductMovementDto { MovementType = 'X', Quantity = 10 }; // Movimento inválido

            var result = _validator.TestValidate(dto);

            result.ShouldHaveValidationErrorFor( o => o.MovementType )
                  .WithErrorMessage( "O tipo de movimento deve ser I para entrada e O para saída" );
        }

        [Fact]
        public void Should_Not_Have_Error_When_MovementType_Is_Valid()
        {
            var dtoEntrada = new ProductMovementDto { MovementType = 'I', Quantity = 10 };
            var dtoSaida = new ProductMovementDto { MovementType = 'O', Quantity = 10 };

            _validator.TestValidate( dtoEntrada ).ShouldNotHaveValidationErrorFor( o => o.MovementType );
            _validator.TestValidate( dtoSaida ).ShouldNotHaveValidationErrorFor( o => o.MovementType );
        }

        [Fact]
        public void Should_Have_Error_When_Quantity_Is_Zero_Or_Less()
        {
            var dto = new ProductMovementDto { MovementType = 'I', Quantity = 0 }; // Quantidade inválida

            var result = _validator.TestValidate(dto);

            result.ShouldHaveValidationErrorFor( o => o.Quantity )
                  .WithErrorMessage( "A quantidade deve ser maior que zero." );
        }

        [Fact]
        public void Should_Not_Have_Error_When_Quantity_Is_Positive()
        {
            var dto = new ProductMovementDto { MovementType = 'I', Quantity = 5 }; // Quantidade válida

            _validator.TestValidate( dto ).ShouldNotHaveValidationErrorFor( o => o.Quantity );
        }
    }
}
