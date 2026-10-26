using Banco.API.Domain.Entities;

namespace Banco.API.Domain.Interfaces
{
    public interface IClienteRepository
    {
        (IEnumerable<ClienteEntity> data, int rowsCount) ObterTodos(int Deslocamento, int RegistroRetornado);
        ClienteEntity? ObterUm(int Id);

        ClienteEntity? Adicionar(ClienteEntity entity);
        ClienteEntity? Editar(int Id, ClienteEntity entity);
        ClienteEntity? Deletar(int Id);
    }
}
