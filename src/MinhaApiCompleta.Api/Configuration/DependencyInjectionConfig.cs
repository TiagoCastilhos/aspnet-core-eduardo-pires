using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MinhaApiCompleta.Business.Interfaces;
using MinhaApiCompleta.Data.Contexts;
using MinhaApiCompleta.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MinhaApiCompleta.Api.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection ResolveDependecies(this IServiceCollection services)
        {
            services.AddScoped<MeuDbContext>();
            services.AddScoped<IFornecedoresRepository, FornecedoresRepository>();

            return services;
        }
    }
}