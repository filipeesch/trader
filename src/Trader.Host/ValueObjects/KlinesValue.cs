using System;

namespace Trader.Host.ValueObjects
{
    public class KlinesValue
    {
        public DateTime OpenTime { get; set; }
        public decimal Open { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Close { get; set; }
        public decimal Volume { get; set; }
        public DateTime CloseTime { get; set; }
        public decimal QuotesAsset { get; set; }
        public int Trades { get; set; }
        public decimal TakerBuyBaseAsset { get; set; }
        public decimal TakerBuyQuoteAsset { get; set; }
    }
}