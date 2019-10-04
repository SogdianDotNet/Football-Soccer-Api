using FL.Common.Base.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace FootballLiga.API.Extensions.Infrastructure
{
    public static class FootballLigaDbStartup
    {
        public static IServiceCollection ConfigureSqlServer(this IServiceCollection services, AppConfiguration config)
        {
            services.AddDbContext
        }
    }
}
