using FluentAssertions;
using Stock_Backend.Application;
using Stock_Backend.Domain;
using System.Net;
using System.Net.Http.Json;

namespace Stock_Backend.IntegrationTests
{
    public class ProductControllerTests :IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public ProductControllerTests( CustomWebApplicationFactory<Program> factory )
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Create_ReturnsOk_WhenProductIsCreatedSuccessfully()
        {
            var command = new CreateProductCommand("Product A", "PN123456", 100.50m);

            var response = await _client.PostAsJsonAsync("/api/Product", command);

            response.StatusCode.Should().Be( HttpStatusCode.OK );

            var result = await response.Content.ReadFromJsonAsync<ReturnCommon>();

            result.Should().NotBeNull();

            result.Success.Should().BeTrue();
        }

        [Fact]
        public async Task Create_ReturnsBadRequest_WhenProductCreationFails()
        {
            var command = new CreateProductCommand("", "", -1); // Valores inválidos

            var response = await _client.PostAsJsonAsync("/api/Product", command);

            response.StatusCode.Should().Be( HttpStatusCode.BadRequest );

            var message = await response.Content.ReadAsStringAsync();

            message.Should().Contain( "obrigatório" );
        }

        [Fact]
        public async Task Get_ReturnsProduct_WhenProductExists()
        {
            var productId = 2;

            var response = await _client.GetAsync($"/api/Product/{productId}");

            response.StatusCode.Should().Be( HttpStatusCode.OK );

            var product = await response.Content.ReadFromJsonAsync<ProductDto>();

            product.Should().NotBeNull();
            product.Id.Should().Be( productId );
        }

        [Fact]
        public async Task Get_ReturnsNotFound_WhenProductDoesNotExist()
        {
            var nonExistentProductId = 9999;

            var response = await _client.GetAsync($"/api/Product/{nonExistentProductId}");

            response.StatusCode.Should().Be( HttpStatusCode.NotFound );

            var message = await response.Content.ReadAsStringAsync();

            message.Should().Contain( "Produto não localizado" );
        }

        [Fact]
        public async Task GetAll_ReturnsAllProducts_WhenProductsExist()
        {
            var response = await _client.GetAsync("/api/Product/all");

            response.StatusCode.Should().Be( HttpStatusCode.OK );

            var products = await response.Content.ReadFromJsonAsync<List<ProductDto>>();

            products.Should().NotBeNull();
            products.Should().HaveCountGreaterThan( 0 );
        }

        [Fact]
        public async Task Update_ReturnsOk_WhenProductIsUpdatedSuccessfully()
        {
            var productId = 2;

            var command = new UpdateProductCommand(productId, "Product Updated", "PN456", 120.75m);

            var response = await _client.PutAsJsonAsync($"/api/Product/{productId}", command);

            response.StatusCode.Should().Be( HttpStatusCode.OK );

            var result = await response.Content.ReadFromJsonAsync<ReturnCommon>();

            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
        }

        [Fact]
        public async Task Update_ReturnsBadRequest_WhenProductIdDoesNotMatch()
        {
            var productId = 1;

            var command = new UpdateProductCommand(2, "Product Updated", "PN456", 120.75m); // ID diferente do ID na URL

            var response = await _client.PutAsJsonAsync($"/api/Product/{productId}", command);

            response.StatusCode.Should().Be( HttpStatusCode.BadRequest );

            var message = await response.Content.ReadAsStringAsync();

            message.Should().Contain( "ID do produto inválido" );
        }

        [Fact]
        public async Task Delete_ReturnsBadRequest_WhenProductDeletionFails()
        {
            var nonExistentProductId = 9999;

            var response = await _client.DeleteAsync($"/api/Product/{nonExistentProductId}");

            response.StatusCode.Should().Be( HttpStatusCode.BadRequest );

            var message = await response.Content.ReadAsStringAsync();

            message.Should().Contain( "Produto não localizado" );
        }
    }
}
