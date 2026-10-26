using Banco.API.Domain.Entities;

namespace Banco.API.Domain.Interfaces
{
    public interface IClienteService
    {
        Task<EnderecoModel?> ObterEnderecoPorCepAsync(string cep);
    }
}
