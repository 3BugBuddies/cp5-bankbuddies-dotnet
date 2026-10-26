using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Banco.API.Domain.Entities
{
    [Table("tb_conta")]
    [Index(nameof(NumeroConta), IsUnique = true, Name = "IDX_conta_numero")]
    public class ContaEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string NumeroConta { get; set; } = string.Empty;
        [Required]
        public string Agencia { get; set; } = string.Empty;
        public double Saldo { get; set; }
        public string TipoConta { get; set; } = string.Empty;

        [ForeignKey(nameof(Cliente))]
        public int ClienteId { get; set; }

        [JsonIgnore]
        public ClienteEntity? Cliente { get; set; }
    }
}
