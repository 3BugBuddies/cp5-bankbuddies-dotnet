using Banco.API.Domain.Entities;

namespace Banco.API.Application.Dtos
{
    public class GetClienteDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Cpf { get; set; } = string.Empty;
        public int Idade { get; set; }

        public IEnumerable<ContaEntity>? Contas { get; set; }

        public List<LinkDto> Links { get; set; } = new();
    }
}
