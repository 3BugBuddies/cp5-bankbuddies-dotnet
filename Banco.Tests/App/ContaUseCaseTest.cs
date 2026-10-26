using Banco.API.Application.UseCases;
using Banco.API.Domain.Entities;
using Banco.API.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;

namespace Banco.Tests.App
{
    public class ContaUseCaseTest
    {
        private readonly Mock<ILogger<ContaUseCase>> _loggerMock;
        private readonly Mock<IContaRepository> _contaRepository;
        private readonly ContaUseCase _contaUseCase;

        public ContaUseCaseTest()
        {
            _loggerMock = new Mock<ILogger<ContaUseCase>>();
            _contaRepository = new Mock<IContaRepository>();
            _contaUseCase = new ContaUseCase(_contaRepository.Object, _loggerMock.Object);
        }

        [Fact]
        [Trait("UseCase", "Conta")]
        public void ObterUmaConta_DeveRetornarConta()
        {
            // Arrange
            int idConta = 1;
            var conta = new ContaEntity { Id = idConta, NumeroConta = "1001", Agencia = "0001", Saldo = 100, TipoConta = "Corrente" };

            _contaRepository.Setup(obj => obj.ObterUma(idConta)).Returns(conta);

            // Act
            var resultado = _contaUseCase.ObterUmaConta(idConta);

            // Assert
            Assert.True(resultado.IsSuccess);
            Assert.Equal(idConta, resultado.Value?.Id);
            Assert.Equal(conta.NumeroConta, resultado.Value?.NumeroConta);
        }
    }
}
