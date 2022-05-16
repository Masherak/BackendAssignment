using System;
using System.Threading.Tasks;
using Assignment.Connection;
using Assignment.Converter;
using Assignment.CSV;
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

			if (!response.IsSuccessful)
			{
				throw new ApplicationException($"The connection did not succeed. Reason: {response.ErrorMessage}");
			}

			var contentAsString = response.Content;
			var converter = new CsvConverter(contentAsString);
			var result = converter.ConvertContent();

			var comparer = new CurrencyComparer();
			var output = comparer.GenerateCombinations(result);

			Console.Write(output);
			Console.WriteLine("Press enter to continue...");
			Console.ReadLine();
		}
	}
}
