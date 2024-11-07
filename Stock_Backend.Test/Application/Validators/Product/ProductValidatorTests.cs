using FluentAssertions;
using Stock_Backend.Application;
using Stock_Backend.Domain;

namespace Stock_Backend.Test.Application
{
    public class ProductValidatorTests
    {
        private readonly ProductValidator _validator;

        public ProductValidatorTests()
        {
            _validator = new ProductValidator();
        }

        [Fact]
        public void DeveRetornarErroParaProdutoInvalido()
        {
            var product = new ProductDto { Name = "", PartNumber = "", AverageCostPrice = -10 };

            var result = _validator.Validate(product);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain( o => o.ErrorMessage.Contains( "Nome é um campo obrigatório." ) );
            result.Errors.Should().Contain( o => o.ErrorMessage.Contains( "PartNumber é um campo obrigatório." ) );
            result.Errors.Should().Contain( o => o.ErrorMessage.Contains( "O preço de custo deve ser maior que zero." ) );
        }
    }
}
