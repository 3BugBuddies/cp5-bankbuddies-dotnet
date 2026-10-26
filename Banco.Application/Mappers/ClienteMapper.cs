using Banco.API.Application.Dtos;
using Banco.API.Domain.Entities;

namespace Banco.API.Application.Mappers
{
    public static class ClienteMapper
    {
        public static ClienteEntity ToClienteEntity(this PostClienteDto obj)
        {
            return new ClienteEntity
            {
                Nome = obj.Nome,
                Email = obj.Email,
                Cpf = obj.Cpf,
                Idade = obj.Idade,
            };
        }

        public static GetClienteDto ToClienteDto(this ClienteEntity obj, IEnumerable<LinkDto> links)
        {
            return new GetClienteDto
            {
                Id = obj.Id,
                Nome = obj.Nome,
                Email = obj.Email,
                Cpf = obj.Cpf,
                Idade = obj.Idade,
                Contas = obj.Contas,
                Links = links?.ToList() ?? Enumerable.Empty<LinkDto>().ToList()
            };
        }
    }
}
