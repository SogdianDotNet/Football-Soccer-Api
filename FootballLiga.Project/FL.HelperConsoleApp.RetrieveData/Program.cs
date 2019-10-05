using FL.Common.Base.Data;
using FL.Data.Domain.Context;
using FL.Data.Repositories;
using FL.HelperConsoleApp.RetrieveData.Model;
using FL.Services.DatabaseAccessors.Players;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace FL.HelperConsoleApp.RetrieveData
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");


            // create service collection
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            var serviceProvider = serviceCollection.BuildServiceProvider();

            var playerService = serviceProvider.GetService<IPlayerService>();

            var lol = playerService.GetPlayerById(100);

            Console.ReadLine();
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<FootballObjectContext>(options => options.UseSqlServer(
                ""), ServiceLifetime.Singleton);

            services.AddSingleton<IDbContext, FootballObjectContext>();
            services.AddSingleton(typeof(IRepository<>), typeof(EfRepository<>));
            services.AddSingleton<IPlayerService, PlayerService>();
        }

        private static void RetrieveAndInsertCompetitions()
        {
            using (var httpClient = new HttpClient())
            {
                using (var httpRequest = new HttpRequestMessage(HttpMethod.Get, "https://api.football-data.org/v2/competitions"))
                {
                    httpRequest.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    httpRequest.Headers.Add("X-Auth-Token", "");

                    using (var httpResponse = httpClient.SendAsync(httpRequest).GetAwaiter().GetResult())
                    {
                        string content = httpResponse.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                        JToken jsonToken = JToken.Parse(content);

                        List<Competition> competitions = jsonToken["competitions"].ToObject<List<Competition>>();

                        var lol = "";
                    }
                }
            }
        }
    }
}
