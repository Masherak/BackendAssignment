using Assignment.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Assignment.Interfaces;

public interface IExchangeRateProvider
{
    Task<IReadOnlyCollection<ExchangeRateModel>> GetCurrencyRates(CancellationToken ct);
}