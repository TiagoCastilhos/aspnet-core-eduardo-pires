using Microsoft.Extensions.DependencyInjection;
using MinhaApiCompleta.Business.Interfaces;
using MinhaApiCompleta.Business.Notifications;
using MinhaApiCompleta.Business.Services;
using MinhaApiCompleta.Data.Contexts;
using MinhaApiCompleta.Data.Repositories;

namespace MinhaApiCompleta.Api.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection ResolveDependecies(this IServiceCollection services)
        {
            services.AddScoped<MeuDbContext>();

            services.AddScoped<IFornecedoresRepository, FornecedoresRepository>();
            services.AddScoped<IEnderecosRepository, EnderecosRepository>();
            services.AddScoped<IProdutosRepository, ProdutosRepository>();

            services.AddScoped<IProdutosService, ProdutosService>();
            services.AddScoped<IFornecedoresService, FornecedoresService>();

            services.AddScoped<INotificador, Notificador>();

            return services;
        }
    }
}