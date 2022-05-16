using Assignment.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.Converter
{
	public class CurrencyComparer
	{
		public StringBuilder Content { get; set; } = new StringBuilder();
		public string GenerateCombinations(CsvDto currencyList)
		{
			Content.AppendLine("Exchange overview for currencies.");

			foreach (var conversion in currencyList.Conversions)
			{
				NormalizeFromInitialCurrency(conversion);
			}

			for (int i = 0; i < currencyList.Conversions.Count; i++)
			{
				for (int j = 0; j < currencyList.Conversions.Count; j++)
				{
					if (i == j)
					{
						continue;
					}

					var normalized = Normalize(currencyList.Conversions[i].Rate, currencyList.Conversions[j].Rate);
					Content.AppendLine(GetComparingText
						(currencyList.Conversions[i].Code, normalized.First,
						currencyList.Conversions[j].Code, normalized.Second));
				}

				Content.AppendLine(GetComparingText
					("CZK", 1, currencyList.Conversions[i].Code, Math.Round(currencyList.Conversions[i].Rate, 3)));
			}

			return Content.ToString();
		}

		/// <summary>Converts the specified currency in an amount comparable to one unit of the compared-to currency, for instance 1 CZK.</summary>
		/// <param name="currency">The currency object.</param>
		public void NormalizeFromInitialCurrency(ConversionRateDto currency)
		{
			if (currency.Amount > 1)
			{
				currency.Rate = currency.Rate / currency.Amount;
				currency.Amount = 1;
			}
		}

		public string GetComparingText(string codeCurr1, decimal rateCurr1, string codeCurr2, decimal rateCurr2)
		{
			return $"Currency: {codeCurr1}, Amount: {rateCurr1}"
					+ " Is equal to: "
					+ $"Currency: {codeCurr2}, Amount: {rateCurr2}";
		}


		public ConversionCoupleDto Normalize(decimal amount1, decimal amount2)
		{
			var divider = amount1;
			var dto = new ConversionCoupleDto();
			dto.First = Math.Round(amount1 / divider, 3);
			dto.Second = Math.Round(amount2 / divider, 3);
			return dto;
		}
	}
}
