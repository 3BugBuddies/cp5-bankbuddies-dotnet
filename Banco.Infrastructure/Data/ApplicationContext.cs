using Banco.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Banco.API.Infrastructure.Data
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {

        }

        public DbSet<UserEntity> Usuario { get; set; }

        public DbSet<ClienteEntity> Cliente { get; set; }

        public DbSet<ContaEntity> Conta { get; set; }
    }
}
