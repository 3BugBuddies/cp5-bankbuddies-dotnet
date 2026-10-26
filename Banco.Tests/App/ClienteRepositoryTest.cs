using Banco.API.Domain.Entities;
using Banco.API.Infrastructure.Data;
using Banco.API.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Banco.Tests.App
{
    public class ClienteRepositoryTest
    {
        private readonly DbContextOptions<ApplicationContext> _options;
        private readonly ApplicationContext _applicationContext;
        private readonly ClienteRepository _clienteRepository;

        public ClienteRepositoryTest()
        {
            _options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase(databaseName: $"ClienteTestDatabase_{Guid.NewGuid()}")
                .Options;

            _applicationContext = new ApplicationContext(_options);

            _applicationContext.Database.EnsureDeleted();
            _applicationContext.Database.EnsureCreated();

            _clienteRepository = new ClienteRepository(_applicationContext);
        }

        [Fact]
        [Trait("Repository", "Cliente")]
        public void ObterTodos_DeveRetornarClientes()
        {
            // Arrange
            var clientes = new List<ClienteEntity>
            {
                new ClienteEntity { Nome = "João", Email = "joao.silva@example.com", Cpf = "11111111111", Idade = 30 },
                new ClienteEntity { Nome = "Maria", Email = "maria.silva@example.com", Cpf = "22222222222", Idade = 25 }
            };

            _applicationContext.Cliente.AddRange(clientes);
            _applicationContext.SaveChanges();

            // Act
            var resultado = _clienteRepository.ObterTodos(0, 10);

            // Assert
            Assert.NotNull(resultado.data);
            Assert.Equal(2, resultado.data.Count());
        }

        [Fact]
        [Trait("Repository", "Cliente")]
        public void Adicionar_DeveAdicionarCliente()
        {
            // Arrange
            var cliente = new ClienteEntity { Nome = "Pedro", Email = "pedro.silva@example.com", Cpf = "33333333333", Idade = 40 };

            // Act
            var resultado = _clienteRepository.Adicionar(cliente);

            // Assert
            var idCliente = resultado?.Id ?? -1;
            var clienteNoDb = _applicationContext.Cliente.FirstOrDefault(c => c.Id == idCliente);

            Assert.NotNull(clienteNoDb);
            Assert.Equal(cliente.Nome, clienteNoDb.Nome);
            Assert.Equal(cliente.Email, clienteNoDb.Email);
        }
    }
}
