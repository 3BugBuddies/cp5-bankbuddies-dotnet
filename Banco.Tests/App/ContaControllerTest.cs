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
    public class ContaCustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        public Mock<IContaUseCase> ContaUseCaseMock { get; } = new();

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.RemoveAll(typeof(IContaUseCase));
                services.AddSingleton(ContaUseCaseMock.Object);
            });
        }
    }

    public class ContaControllerTest : IClassFixture<ContaCustomWebApplicationFactory>
    {
        private readonly ContaCustomWebApplicationFactory _factory;

        public ContaControllerTest(ContaCustomWebApplicationFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        [Trait("Controller", "Conta")]
        public async Task Get_DeveRetornar200()
        {
            // Arrange
            var conta = new GetContaDto { Id = 1, NumeroConta = "1001", Agencia = "0001", Saldo = 100, TipoConta = "Corrente" };

            var operationResult = OperationResult<GetContaDto?>.Success(conta);

            _factory.ContaUseCaseMock.Setup(x => x.ObterUmaConta(1)).Returns(operationResult);

            using var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/api/conta/1");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
