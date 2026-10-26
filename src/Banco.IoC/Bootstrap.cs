using Banco.API.Application.Interfaces;
using Banco.API.Application.UseCases;
using Banco.API.Domain.Interfaces;
using Banco.API.Infrastructure.Data;
using Banco.API.Infrastructure.Data.Repositories;
using Banco.API.Infrastructure.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Banco.API.Infrastructure.IoC
{
    public static class Bootstrap
    {
        public static void AddIoC(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationContext>(options => {
                options.UseOracle(configuration.GetConnectionString("Oracle"));
            });

            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IUserUseCase, UserUseCase>();

            services.AddTransient<IClienteRepository, ClienteRepository>();
            services.AddTransient<IClienteUseCase, ClienteUseCase>();
            services.AddTransient<IClienteService, ClienteService>();

            services.AddTransient<IContaRepository, ContaRepository>();
            services.AddTransient<IContaUseCase, ContaUseCase>();
        }
    }
}
