namespace Assignment.Models;

public record ExchangeRateModel(
    string CountryName,
    string CurrencyName,
    uint Amount,
    string CurrencyCode,
    decimal Rate)
{
    /// <summary>
    /// Parameterless ctor is required by CsvHelper component.
    /// </summary>
    private ExchangeRateModel() : this(string.Empty, string.Empty, 0, string.Empty, 0m)
    { }
}
