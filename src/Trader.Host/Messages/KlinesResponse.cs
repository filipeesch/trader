using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Trader.Host.Helpers;
using Trader.Host.ValueObjects;

namespace Trader.Host.Messages
{
    public class KlinesResponse
    {
        public List<KlinesValue> Values { get; set; }

        public static KlinesResponse Parse(List<List<object>> values)
        {
            return new KlinesResponse
            {
                Values = values.Select(x => new KlinesValue
                {
                    OpenTime = DateHelpers.FromBinanceDate(Convert.ToInt64(x[0])),
                    Open = Convert.ToDecimal(x[1], CultureInfo.InvariantCulture),
                    High = Convert.ToDecimal(x[2], CultureInfo.InvariantCulture),
                    Low = Convert.ToDecimal(x[3], CultureInfo.InvariantCulture),
                    Close = Convert.ToDecimal(x[4], CultureInfo.InvariantCulture),
                    Volume = Convert.ToDecimal(x[5], CultureInfo.InvariantCulture),
                    CloseTime = DateHelpers.FromBinanceDate(Convert.ToInt64(x[6])),
                    QuotesAsset = Convert.ToDecimal(x[7], CultureInfo.InvariantCulture),
                    Trades = Convert.ToInt32(x[8]),
                    TakerBuyBaseAsset = Convert.ToDecimal(x[10], CultureInfo.InvariantCulture),
                    TakerBuyQuoteAsset = Convert.ToDecimal(x[11], CultureInfo.InvariantCulture)
                }).ToList()
            };
        }
    }
}