using Assignment.Config;
using Assignment.Interfaces;
using Assignment.Models;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Assignment.Services;

internal class CnbExchangeRateProvider : IExchangeRateProvider
{
    readonly IHttpClientFactory _httpClientFactory;
    readonly ILogger<CnbExchangeRateProvider> _logger;
    readonly CnbSettings _cnbSettings;

    public CnbExchangeRateProvider(
        IHttpClientFactory httpClientFactory,
        IOptions<CnbSettings> cnbSettings,
        ILogger<CnbExchangeRateProvider> logger)
    {
        _httpClientFactory = httpClientFactory;
        //_configuration = configuration;
        _logger = logger;
        _cnbSettings = cnbSettings.Value;
    }

    public async Task<IReadOnlyCollection<ExchangeRateModel>> GetCurrencyRates(CancellationToken ct)
    {
        using Stream dataStream = await DownloadCurrentRates(ct);

        using StreamReader streamReader = new StreamReader(dataStream);
        CsvConfiguration csvConfiguration = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            Delimiter = "|",
        };

        using CsvReader csvReader = new CsvReader(streamReader, csvConfiguration);
        csvReader.Context.RegisterClassMap<ExchangeRateModelMap>();

        if (csvReader.Read())
        {
            if (csvReader.Read()) // 2nd call to skip timestamp record in input data.
            {
                if (csvReader.ReadHeader())
                {
                    var records = csvReader.GetRecords<ExchangeRateModel>().ToList().AsReadOnly();
                    _logger.LogInformation($"Downloaded {records.Count} records.");
                    return records;
                }
                else
                {
                    throw new InvalidOperationException($"No records found below header in data feed");
                }
            }
            else
            {
                throw new InvalidOperationException($"CSV data header not found in data feed");
            }
        }
        throw new InvalidOperationException($"Data feed is empty");
    }

    private async Task<Stream> DownloadCurrentRates(CancellationToken ct)
    {
        _logger.LogInformation($"Start download exchange rates from {_cnbSettings.EndpointUrl} ...");

        using HttpClient httpClient = _httpClientFactory.CreateClient();

        return await httpClient.GetStreamAsync(_cnbSettings.EndpointUrl, ct);
    }

    /// <summary>
    /// Map CSV fields to model/dto record.
    /// </summary>
    private class ExchangeRateModelMap : ClassMap<ExchangeRateModel>
    {
        public ExchangeRateModelMap()
        {
            Map(p => p.CountryName).Index(0);
            Map(p => p.CurrencyName).Index(1);
            Map(p => p.Amount).Index(2);
            Map(p => p.CurrencyCode).Index(3);
            Map(p => p.Rate).Index(4);
        }
    }
}
