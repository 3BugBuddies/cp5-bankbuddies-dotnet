using Banco.API.Application.Dtos;
using Banco.API.Application.Interfaces;
using Banco.API.Domain.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using System.Net;

namespace Banco.Tests.App
{
    public class ClienteCustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        public Mock<IClienteUseCase> ClienteUseCaseMock { get; } = new();

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.RemoveAll(typeof(IClienteUseCase));
                services.AddSingleton(ClienteUseCaseMock.Object);
            });
        }
    }

    public class ClienteControllerTest : IClassFixture<ClienteCustomWebApplicationFactory>
    {
        private readonly ClienteCustomWebApplicationFactory _factory;

        public ClienteControllerTest(ClienteCustomWebApplicationFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        [Trait("Controller", "Cliente")]
        public async Task Get_DeveRetornar200()
        {
            // Arrange
            var cliente = new GetClienteDto { Id = 1, Nome = "João", Email = "joao.silva@example.com" };

            var operationResult = OperationResult<GetClienteDto?>.Success(cliente);

            _factory.ClienteUseCaseMock.Setup(x => x.ObterUmCliente(1)).Returns(operationResult);

            using var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/api/cliente/1");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
