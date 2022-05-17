using Assignment.Interfaces;
using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Assignment
{
	public class Worker : IWorker
	{
		private async Task<SortedDictionary<string, RateRecord>> LoadData(string url)
		{
			using (var client = new HttpClient())
			using (var response = await client.GetStreamAsync(url).ConfigureAwait(false))
			{
				using (StreamReader reader = new StreamReader(response, System.Text.Encoding.GetEncoding("us-ascii")))
				{
					var a = reader.ReadLine();

					CsvConfiguration csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
					{
						Delimiter = "|",
					};

					using (CsvReader csvReader = new CsvReader(reader, csvConfig))
					{
						var rates = new SortedDictionary<string, RateRecord>();
						foreach (RateRecord rate in csvReader.GetRecords<RateRecord>())
						{
							rates.Add(rate.Code, rate);
						}

						rates.Add("CZK", new RateRecord
						{
							Country = "Czechia",
							Currency = "koruna",
							Amount = 1,
							Code = "CZK",
							Rate = 1
						});

						return rates;
					}
				}
			}
		}

		private async Task PrintData(SortedDictionary<string, RateRecord> rates)
		{
			// it can also be linq, but foreach cycle looks faster for this case
			// (I'm just guessing)
			foreach (var rate in rates)
			{
				foreach (var rate1 in rates)
				{
					if (rate.Key == rate1.Key)
					{
						continue;
					}

					// division by zero is not checked due to performance, but can be TODO
					double r = (rate.Value.Rate / rate.Value.Amount) / (rate1.Value.Rate / rate1.Value.Amount);
					await _stringWriter.Write(string.Format("{0,2}", 1) + " " + rate.Value.Code + " = " + string.Format("{0,12:N6}", r) + " " + rate1.Value.Code);
				}
			}

			await _stringWriter.Finish();
		}

		private IStringWriter _stringWriter;
		public async Task RunAsync(IStringWriter stringWriter)
		{
			_stringWriter = stringWriter;
			await PrintData(await LoadData(String.Format(Properties.Resources.WebDataSourceName, DateTime.Now.ToString("dd.MM.yyyy"))));
		}
	}
}
