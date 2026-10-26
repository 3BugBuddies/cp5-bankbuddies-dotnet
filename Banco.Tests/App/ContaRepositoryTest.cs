using Banco.API.Domain.Entities;
using Banco.API.Infrastructure.Data;
using Banco.API.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Banco.Tests.App
{
    public class ContaRepositoryTest
    {
        private readonly DbContextOptions<ApplicationContext> _options;
        private readonly ApplicationContext _applicationContext;
        private readonly ContaRepository _contaRepository;

        public ContaRepositoryTest()
        {
            _options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase(databaseName: $"ContaTestDatabase_{Guid.NewGuid()}")
                .Options;

            _applicationContext = new ApplicationContext(_options);

            _applicationContext.Database.EnsureDeleted();
            _applicationContext.Database.EnsureCreated();

            _contaRepository = new ContaRepository(_applicationContext);
        }

        [Fact]
        [Trait("Repository", "Conta")]
        public void ObterTodos_DeveRetornarContas()
        {
            // Arrange
            var contas = new List<ContaEntity>
            {
                new ContaEntity { NumeroConta = "1001", Agencia = "0001", Saldo = 100, TipoConta = "Corrente" },
                new ContaEntity { NumeroConta = "1002", Agencia = "0001", Saldo = 500, TipoConta = "Poupanca" }
            };

            _applicationContext.Conta.AddRange(contas);
            _applicationContext.SaveChanges();

            // Act
            var resultado = _contaRepository.ObterTodos(0, 10);

            // Assert
            Assert.NotNull(resultado.data);
            Assert.Equal(2, resultado.data.Count());
        }

        [Fact]
        [Trait("Repository", "Conta")]
        public void Adicionar_DeveAdicionarConta()
        {
            // Arrange
            var conta = new ContaEntity { NumeroConta = "2001", Agencia = "0002", Saldo = 1000, TipoConta = "Corrente" };

            // Act
            var resultado = _contaRepository.Adicionar(conta);

            // Assert
            var idConta = resultado?.Id ?? -1;
            var contaNoDb = _applicationContext.Conta.FirstOrDefault(c => c.Id == idConta);

            Assert.NotNull(contaNoDb);
            Assert.Equal(conta.NumeroConta, contaNoDb.NumeroConta);
            Assert.Equal(conta.Saldo, contaNoDb.Saldo);
        }
    }
}
