using Banco.API.Application.Dtos;
using Banco.API.Domain.Entities;

namespace Banco.API.Application.Interfaces
{
    public interface IUserUseCase
    {
        Task<OperationResult<UserEntity?>> AdicionarUserAsync(PostUserDto entity);
        Task<OperationResult<GetAuthDto?>> AutenticarUserAsync(PostAuthDto model);
    }
}
