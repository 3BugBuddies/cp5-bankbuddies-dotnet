using Banco.API.Application.Dtos;
using Banco.API.Domain.Entities;

namespace Banco.API.Application.Mappers
{
    public static class ContaMapper
    {
        public static ContaEntity ToContaEntity(this PostContaDto obj)
        {
            return new ContaEntity
            {
                NumeroConta = obj.NumeroConta,
                Agencia = obj.Agencia,
                Saldo = obj.Saldo,
                TipoConta = obj.TipoConta,
                ClienteId = obj.ClienteId,
            };
        }

        public static GetContaDto ToContaDto(this ContaEntity obj, IEnumerable<LinkDto> links)
        {
            return new GetContaDto
            {
                Id = obj.Id,
                NumeroConta = obj.NumeroConta,
                Agencia = obj.Agencia,
                Saldo = obj.Saldo,
                TipoConta = obj.TipoConta,
                ClienteId = obj.ClienteId,
                Links = links?.ToList() ?? Enumerable.Empty<LinkDto>().ToList()
            };
        }
    }
}
