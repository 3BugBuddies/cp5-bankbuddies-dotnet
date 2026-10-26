using Banco.API.Application.UseCases;
using Banco.API.Domain.Entities;
using Banco.API.Domain.Errors;
using Banco.API.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;

namespace Banco.Tests.Unit
{
    public class ClienteUseCaseTest
    {
        private readonly Mock<ILogger<ClienteUseCase>> _loggerMock;
        private readonly Mock<IClienteRepository> _clienteRepository;
        private readonly Mock<IClienteService> _clienteService;
        private readonly ClienteUseCase _clienteUseCase;

        public ClienteUseCaseTest()
        {
            _loggerMock = new Mock<ILogger<ClienteUseCase>>();
            _clienteRepository = new Mock<IClienteRepository>();
            _clienteService = new Mock<IClienteService>();
            _clienteUseCase = new ClienteUseCase(_clienteRepository.Object, _loggerMock.Object, _clienteService.Object);
        }

        [Fact]
        [Trait("UseCase", "Cliente")]
        public void ObterUmCliente_DeveRetornarCliente()
        {
            // Arrange
            int idCliente = 1;
            var cliente = new ClienteEntity { Id = idCliente, Nome = "Carlos", Email = "cliente1@teste.com" };

            _clienteRepository.Setup(obj => obj.ObterUm(idCliente)).Returns(cliente);

            // Act
            var resultado = _clienteUseCase.ObterUmCliente(idCliente);

            // Assert
            Assert.True(resultado.IsSuccess);
            Assert.Equal(idCliente, resultado.Value?.Id);
            Assert.Equal(cliente.Nome, resultado.Value?.Nome);
        }

        [Fact]
        [Trait("UseCase", "Cliente")]
        public void ObterUmCliente_DeveRetornarNotFound_QuandoNaoExiste()
        {
            // Arrange
            int idCliente = 99;

            _clienteRepository.Setup(obj => obj.ObterUm(idCliente)).Throws(new NoContentException("Não foi encontrado o cliente"));

            // Act
            var resultado = _clienteUseCase.ObterUmCliente(idCliente);

            // Assert
            Assert.False(resultado.IsSuccess);
            Assert.Equal(404, resultado.StatusCode);
        }
    }
}
