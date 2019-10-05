using FL.Common.Base.Configuration;
using FL.Common.Base.Data;
using FL.Data.Domain.Context;
using FL.Data.Repositories;
using FootballLiga.API.Extensions.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace FootballLiga.API.Extensions.Extensions
{
    /// <summary>
    /// Represents extensions of IServiceCollection
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add services to the application and configure service provider
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        /// <param name="configuration">Configuration of the application</param>
        /// <param name="hostingEnvironment">Hosting environment</param>
        /// <returns>Configured service provider</returns>
        public static IServiceCollection ConfigureApplicationServices(this IServiceCollection services,
            IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            //most of API providers require TLS 1.2 nowadays
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            var applicationConfig = services.ConfigureStartupConfig<AppConfiguration>(configuration.GetSection("FootballAppConfig"));

            services.AddMemoryCache();

            services.ConfigureDependencyInjections();

            services.ConfigureSqlServer(applicationConfig);

            return services;
        }

        public static IServiceCollection ConfigureDependencyInjections(this IServiceCollection services)
        {
            services.AddSingleton<IDbContext, FootballObjectContext>();
            services.AddSingleton(typeof(IRepository<>), typeof(EfRepository<>));
            return services;
        }

        /// <summary>
        /// Create, bind and register as service the specified configuration parameters 
        /// </summary>
        /// <typeparam name="TConfig">Configuration parameters</typeparam>
        /// <param name="services">Collection of service descriptors</param>
        /// <param name="configuration">Set of key/value application configuration properties</param>
        /// <returns>Instance of configuration parameters</returns>
        public static TConfig ConfigureStartupConfig<TConfig>(this IServiceCollection services, IConfiguration configuration) where TConfig : class, new()
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            //create instance of config
            var config = new TConfig();

            //bind it to the appropriate section of configuration
            configuration.Bind(config);

            //and register it as a service
            services.AddSingleton(config);

            return config;
        }
    }
}
