using Assignment.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyCsvParser.Mapping;

namespace Assignment.CSV
{
    public class ConversionRateDtoMapping : CsvMapping<ConversionRateDto>
    {
        public ConversionRateDtoMapping()
            : base()
        {
            MapProperty(0, x => x.Country);
            MapProperty(1, x => x.Currency);
            MapProperty(2, x => x.Amount);
            MapProperty(3, x => x.Code);
            MapProperty(4, x => x.Rate);
        }
    }
}
