using FL.Common.Base.Configuration;
using FL.Data.Domain.Context;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace FootballLiga.API.Extensions.Infrastructure
{
    public static class FootballLigaDbStartup
    {
        public static IServiceCollection ConfigureSqlServer(this IServiceCollection services, AppConfiguration config)
        {
            services.AddDbContext<FootballObjectContext>(options => options.UseSqlServer(config.ConnectionStrings.AzureSqlDatabase), ServiceLifetime.Singleton);

            return services;
        }
    }
}
