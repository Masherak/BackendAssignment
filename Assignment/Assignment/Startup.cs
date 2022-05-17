using Assignment.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Assignment
{
	public class Startup
	{
		private readonly IConfiguration _configuration;
		private readonly IServiceProvider _provider;

		public IWorker GetWorker() => _provider.GetRequiredService<IWorker>();
		public IStringWriter GetStringWriter() => _provider.GetRequiredService<IStringWriter>();

		public Startup()
		{
			var configurationBuilder = new ConfigurationBuilder();

			_configuration = configurationBuilder.Build();

			_provider = ConfigureServices().BuildServiceProvider();
		}

		private ServiceCollection ConfigureServices()
		{
			var services = new ServiceCollection();

			services.AddSingleton<IWorker, Worker>();
			services.AddSingleton<IStringWriter, StringConsoleWriter>();

			return services;
		}
	}
}
