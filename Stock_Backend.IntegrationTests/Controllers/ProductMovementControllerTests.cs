using MediatR;
using Moq;
using Stock_Backend.Application;
using Stock_Backend.Domain;
using System.Net;
using System.Net.Http.Json;

namespace Stock_Backend.IntegrationTests
{
    public class ProductMovementControllerTests :IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly Mock<IMediator> _mediatorMock;

        public ProductMovementControllerTests( CustomWebApplicationFactory<Program> factory )
        {
            _mediatorMock = new Mock<IMediator>();

            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Update_ReturnsOk_WhenMovementIsSuccessful()
        {
            var command = new ProductMovementCommand
            (
                ProductId: 2, 
                Quantity: 10,     
                MovementType: 'I', 
                Message: "Entrada de produtos"
            );

            var response = await _client.PutAsJsonAsync("/api/ProductMovement/setMovement", command);

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<ReturnCommon>();

            Assert.NotNull( result );
            Assert.True( result.Success ); 
        }

        [Fact]
        public async Task Update_ReturnsBadRequest_WhenCommandIsNull()
        {
            var content = new StringContent("{}", System.Text.Encoding.UTF8, "application/json");

            var response = await _client.PutAsync("/api/ProductMovement/setMovement", content);

            Assert.Equal( HttpStatusCode.BadRequest, response.StatusCode );

            var result = await response.Content.ReadFromJsonAsync<ReturnCommon>();

            Assert.NotNull( result );
            Assert.False( result.Success ); 
        }

        [Fact]
        public async Task Update_ReturnsBadRequest_WhenMovementFails()
        {
            var command = new ProductMovementCommand
             (
                 ProductId: -1, // ID inválido
                 Quantity: 10,
                 MovementType: 'O',
                 Message: "Saída de produtos"
             );

            var response = await _client.PutAsJsonAsync("/api/ProductMovement/setMovement", command);

            Assert.Equal( HttpStatusCode.BadRequest, response.StatusCode );

            var result = await response.Content.ReadFromJsonAsync<ReturnCommon>();

            Assert.NotNull( result );
            Assert.False( result.Success ); // Espera que o movimento falhe
        }

        [Fact]
        public async Task GetDailyProductMovement_ReturnsOk_WhenDataExists()
        {
            var date = new DateTime(2024, 11, 05);

            var response = await _client.GetAsync($"/api/ProductMovement/dailyMovement?date={date:yyyy-MM-dd}");

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<List<ProductMovementDto>>();

            Assert.NotNull( result );
            Assert.NotEmpty( result );
        }

        [Fact]
        public async Task GetDailyProductMovement_ReturnsNotFound_WhenNoDataExists()
        {
            var date = new DateTime(1900, 01, 01);

            var response = await _client.GetAsync($"/api/ProductMovement/dailyMovement?date={date:yyyy-MM-dd}");

            Assert.Equal( HttpStatusCode.NotFound, response.StatusCode );

            var message = await response.Content.ReadAsStringAsync();

            Assert.Equal( "Sem movimentação de produtos na data selecionada!", message ); 
        }


    }
}
