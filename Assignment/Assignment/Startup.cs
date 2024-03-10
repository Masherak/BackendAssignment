using Assignment.Config;
using Assignment.Interfaces;
using Assignment.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;

namespace Assignment
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider _provider;

        public IWorker GetWorker() => _provider.GetRequiredService<IWorker>();

        public Startup()
        {
            var configurationBuilder = new ConfigurationBuilder();

            _configuration = configurationBuilder
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            _provider = ConfigureServices().BuildServiceProvider();
        }

        private ServiceCollection ConfigureServices()
        {
            var services = new ServiceCollection();

            services.AddSingleton<IWorker, Worker>();

            services.AddHttpClient();
            services.AddTransient<IExchangeRateProvider, CnbExchangeRateProvider>();
            services.Configure<CnbSettings>(_configuration.GetSection(nameof(CnbSettings)));

            return services;
        }
    }
}
