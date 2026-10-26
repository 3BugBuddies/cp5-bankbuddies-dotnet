using System.ComponentModel.DataAnnotations;

namespace Banco.API.Application.Dtos
{
    public class PostClienteDto
    {
        [Required]
        public string Nome { get; set; } = string.Empty;
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Cpf { get; set; } = string.Empty;
        public int Idade { get; set; }
    }
}
