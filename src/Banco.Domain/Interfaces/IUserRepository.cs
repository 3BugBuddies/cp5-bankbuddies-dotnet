using Banco.API.Domain.Entities;

namespace Banco.API.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<UserEntity?> AdicionarAsync(UserEntity entity);
        Task<UserEntity?> AutenticarAsync(string email, string password);
    }
}
