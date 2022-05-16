using Assignment.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyCsvParser;

namespace Assignment.CSV
{
	public class CsvConverter
	{
		public CsvConverter(string csvContent)
		{
			_csvContent = csvContent;
		}

		private string _csvContent { get; set; }

        public CsvDto ConvertContent()
        {
            CsvParserOptions csvParserOptions = new CsvParserOptions(true, '|');
            CsvReaderOptions csvReaderOptions = new CsvReaderOptions(new[] { Environment.NewLine, "\n", "\r" });
            ConversionRateDtoMapping csvMapper = new ConversionRateDtoMapping();
            CsvParser<ConversionRateDto> csvParser = new CsvParser<ConversionRateDto>(csvParserOptions, csvMapper);

            var result = csvParser
                .ReadFromString(csvReaderOptions, _csvContent)
                .ToList();

			if (result.Count == 0)
			{
                throw new ApplicationException($"The Csv data conversion did not succeed!");
            }

            return new CsvDto
            {
                Conversions = new List<ConversionRateDto>(result.Where(res => res.IsValid).Select(resu => resu.Result).ToList())
            };
        }
    }
}

