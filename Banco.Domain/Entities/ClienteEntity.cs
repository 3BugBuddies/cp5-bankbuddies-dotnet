using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Banco.API.Domain.Entities
{
    [Table("tb_cliente")]
    [Index(nameof(Email), IsUnique = true, Name = "IDX_cliente_email")]
    public class ClienteEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(150, MinimumLength = 3)]
        public string Nome { get; set; } = string.Empty;
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Cpf { get; set; } = string.Empty;
        public int Idade { get; set; }

        [JsonIgnore]
        public ICollection<ContaEntity>? Contas { get; set; }
    }
}
