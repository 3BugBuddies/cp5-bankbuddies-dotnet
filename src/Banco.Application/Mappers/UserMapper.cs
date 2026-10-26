using Banco.API.Application.Dtos;
using Banco.API.Domain.Entities;

namespace Banco.API.Application.Mappers
{
    public static class UserMapper
    {
        public static UserEntity ToUserEntity(this PostUserDto obj)
        {
            return new UserEntity
            {
                Nome = obj.Nome,
                Email = obj.Email,
                Password = obj.Password,
                Role = obj.Role,
            };
        }
    }
}
