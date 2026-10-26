using System.ComponentModel.DataAnnotations;

namespace Banco.API.Application.Dtos
{
    public class PostContaDto
    {
        [Required]
        public string NumeroConta { get; set; } = string.Empty;
        [Required]
        public string Agencia { get; set; } = string.Empty;
        public double Saldo { get; set; }
        [Required]
        public string TipoConta { get; set; } = string.Empty;
        public int ClienteId { get; set; }
    }
}
