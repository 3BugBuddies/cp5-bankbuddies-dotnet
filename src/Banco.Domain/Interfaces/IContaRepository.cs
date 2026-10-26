using Banco.API.Domain.Entities;

namespace Banco.API.Domain.Interfaces
{
    public interface IContaRepository
    {
        (IEnumerable<ContaEntity> data, int rowsCount) ObterTodos(int Deslocamento, int RegistroRetornado);
        IEnumerable<ContaEntity> ObterTodasPorCliente(int clienteId);
        ContaEntity? ObterUma(int Id);

        ContaEntity? Adicionar(ContaEntity entity);
        ContaEntity? Editar(int Id, ContaEntity entity);
        ContaEntity? Deletar(int Id);
    }
}
