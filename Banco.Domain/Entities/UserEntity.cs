using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Banco.API.Domain.Entities
{
    [Table("tb_usuario")]
    public class UserEntity
    {
        [Key]
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }
}
