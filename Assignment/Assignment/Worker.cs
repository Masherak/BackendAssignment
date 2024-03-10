using Assignment.Interfaces;
using Assignment.Models;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Assignment
{
    public class Worker : IWorker
    {
        readonly IExchangeRateProvider _exchangeRateProvider;

        public Worker(IExchangeRateProvider exchangeRateProvider)
        {
            _exchangeRateProvider = exchangeRateProvider;
        }

        public async Task RunAsync(CancellationToken ct)
        {
            ExchangeRateModel[] exchangeRates =
                (await _exchangeRateProvider.GetCurrencyRates(ct)).ToArray();

            decimal[,] allRates = CalculateMatrix(exchangeRates);

            PrintToConsole(exchangeRates, allRates);

            Console.WriteLine("Completed. Press any key to exit");
            Console.ReadKey();
        }

        private static void PrintToConsole(in ExchangeRateModel[] exchangeRates, in decimal[,] allRates)
        {
            for (int r = 0; r < allRates.GetLength(0); r++)
            {
                Console.WriteLine($"{Environment.NewLine}{exchangeRates[r].CountryName}-{exchangeRates[r].CurrencyCode} exchange rates:");
                // print header with currency codes.
                for (int c = 0; c < allRates.GetLength(1); c++)
                {
                    if (r != c)
                    {
                        Console.Write("{0, -10}", exchangeRates[c].CurrencyCode);
                    }
                }
                Console.WriteLine();
                // print currency rates.
                for (int c = 0; c < allRates.GetLength(1); c++)
                {
                    if (r != c)
                    {
                        Console.Write("{0, -10}", allRates[r, c]);
                    }
                }
                Console.WriteLine();
            }
        }

        private static decimal[,] CalculateMatrix(in ExchangeRateModel[] exchangeRates)
        {
            decimal[,] allRates = new decimal[exchangeRates.Length, exchangeRates.Length];

            for (int r = 0; r < exchangeRates.Length; r++)
            {
                for (int c = r; c < exchangeRates.Length; c++)
                {
                    if (c == r)
                    {
                        allRates[r, c] = 1;
                    }
                    else
                    {
                        decimal temp = (exchangeRates[r].Rate / exchangeRates[r].Amount) / (exchangeRates[c].Rate / exchangeRates[c].Amount);
                        allRates[r, c] = Math.Round(temp, 3, MidpointRounding.ToEven);
                        allRates[c, r] = Math.Round(1 / temp, 3, MidpointRounding.ToEven);
                    }
                }
            }
            return allRates;
        }
    }
}
