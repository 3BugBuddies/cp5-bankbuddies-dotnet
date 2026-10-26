using Banco.API.Application.Dtos;
using Banco.API.Domain.Entities;

namespace Banco.API.Application.Interfaces
{
    public interface IContaUseCase
    {
        OperationResult<PageResponseDto<GetContaDto>> ObterTodasContas(int Deslocamento, int RegistroRetornado);
        OperationResult<IEnumerable<ContaEntity>> ObterContasPorCliente(int clienteId);
        OperationResult<GetContaDto?> ObterUmaConta(int Id);
        OperationResult<ContaEntity?> AdicionarConta(PostContaDto entity);
        OperationResult<ContaEntity?> EditarConta(int Id, PostContaDto entity);
        OperationResult<ContaEntity?> DeletarConta(int Id);
    }
}
