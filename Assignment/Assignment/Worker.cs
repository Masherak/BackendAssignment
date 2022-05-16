using System;
using System.Threading.Tasks;
using Assignment.Connection;
using Assignment.Interfaces;

namespace Assignment
{
	public class Worker : IWorker
	{
		public async Task RunAsync()
		{
			var client = new ApiClient();
			var response = await client.FetchCurrencyExchangeRates("/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt",
				DateTime.UtcNow);
		}
	}
}
