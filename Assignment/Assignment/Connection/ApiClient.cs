using Assignment.Extensions;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.Connection
{
	public class ApiClient
	{
		public ApiClient(string basePath = "https://www.cnb.cz/en")
		{
			BasePath = basePath;
			RestClient = new RestClient(BasePath);
		}

		/// <summary>
		/// Gets or sets the base path.
		/// </summary>
		/// <value>The base path</value>
		public string BasePath { get; set; }

		/// <summary>
		/// Gets or sets the RestClient.
		/// </summary>
		/// <value>An instance of the RestClient</value>
		public RestClient RestClient { get; set; }


		public async Task<RestResponse> FetchCurrencyExchangeRates(string path, DateTime date)
		{
			var request = new RestRequest(path, Method.Get);

			request.AddParameter("date", date.ToApiFormattedString(), ParameterType.GetOrPost);

			return await RestClient.ExecuteAsync(request);
		}
	}
}
