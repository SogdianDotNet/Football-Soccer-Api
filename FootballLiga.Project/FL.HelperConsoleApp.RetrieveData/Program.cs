using FL.Common.Base.Data;
using FL.Data.Domain.Context;
using FL.Data.Repositories;
using FL.HelperConsoleApp.RetrieveData.Model;
using FL.Services.DatabaseAccessors.Players;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace FL.HelperConsoleApp.RetrieveData
{
    class Program
    {
        private static ConfigurationSettings configuration;

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var builder = new ConfigurationBuilder()
                .AddJsonFile("configuration.json", optional: false, reloadOnChange: true);

            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("configuration.json", true, true)
                .Build();

            var configurationSettings = new ConfigurationSettings();

            config.GetSection("KeysAndUrls").Bind(configurationSettings);

            // create service collection
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton(configurationSettings);
            ConfigureServices(serviceCollection);

            var serviceProvider = serviceCollection.BuildServiceProvider();
            configuration = serviceProvider.GetService<ConfigurationSettings>();

            var lol = RetrieveAndInsertCompetitions();

            Console.ReadLine();
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<FootballObjectContext>(options => options.UseSqlServer(configuration.AzureSqlDatabase), ServiceLifetime.Singleton);

            services.AddSingleton<IDbContext, FootballObjectContext>();
            services.AddSingleton(typeof(IRepository<>), typeof(EfRepository<>));
            services.AddSingleton<IPlayerService, PlayerService>();
        }

        private static List<Competition> RetrieveAndInsertCompetitions()
        {
            var competitions = new List<Competition>();
            using (var httpClient = new HttpClient())
            {
                string url = string.Format(configuration.GetListOfLeagues, configuration.ApiKey, configuration.AppSecret);
                using (var httpRequest = new HttpRequestMessage(HttpMethod.Get, url))
                {
                    httpRequest.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                    using (var httpResponse = httpClient.SendAsync(httpRequest).GetAwaiter().GetResult())
                    {
                        string content = httpResponse.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                        JToken jsonToken = JToken.Parse(content);

                        competitions = jsonToken["data"]["league"].ToObject<List<Competition>>();
                    }
                }
            }
            return competitions;
        }

        private static List<Team> RetrieveAllTeamsByCompetitionId(int competitionId)
        {
            var teams = new List<Team>();
            using (var httpClient = new HttpClient())
            {
                string url = "https://api.football-data.org/v2/competitions/{0}/teams";
                using (var httpRequest = new HttpRequestMessage(HttpMethod.Get, String.Format(url, competitionId)))
                {
                    httpRequest.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    httpRequest.Headers.Add("X-Auth-Token", "");

                    using (var httpResponse = httpClient.SendAsync(httpRequest).GetAwaiter().GetResult())
                    {
                        string content = httpResponse.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                        JToken jsonToken = JToken.Parse(content);

                        teams = jsonToken["teams"].ToObject<List<Team>>();
                    }
                }
            }
            return teams;
        }
    }
}
