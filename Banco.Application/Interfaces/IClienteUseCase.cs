using Banco.API.Application.Dtos;
using Banco.API.Domain.Entities;

namespace Banco.API.Application.Interfaces
{
    public interface IClienteUseCase
    {
        OperationResult<PageResponseDto<GetClienteDto>> ObterTodosClientes(int Deslocamento, int RegistroRetornado);
        OperationResult<GetClienteDto?> ObterUmCliente(int Id);
        OperationResult<ClienteEntity?> AdicionarCliente(PostClienteDto entity);
        OperationResult<ClienteEntity?> EditarCliente(int Id, PostClienteDto entity);
        OperationResult<ClienteEntity?> DeletarCliente(int Id);

        Task<OperationResult<EnderecoModel?>> ObterEnderecoPorCepAsync(string cep);
    }
}
