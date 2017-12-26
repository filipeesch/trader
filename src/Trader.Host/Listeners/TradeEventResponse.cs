using System;
using System.Globalization;

namespace Trader.Host.Listeners
{
    public class TradeEventResponse
    {
        public string EventType { get; set; }
        public DateTime EventTime { get; set; }
        public string Symbol { get; set; }
        public int AggregatedTradeId { get; set; }
        public decimal Price { get; set; }
        public decimal Quantity { get; set; }
        public int FirstBreakdownTradeId { get; set; }
        public int LastBreakdownTradeId { get; set; }
        public DateTime TradeTime { get; set; }
        public bool BuyerIsAMaker { get; set; }

        public static TradeEventResponse Parse(TradeEventRawResponse x)
        {
            return new TradeEventResponse
            {
                EventType = x.EventType,
                EventTime = new DateTime(1970, 1, 1) + TimeSpan.FromMilliseconds(x.EventTime),
                Symbol = x.Symbol,
                AggregatedTradeId = x.AggregatedTradeId,
                Price = Convert.ToDecimal(x.Price, CultureInfo.InvariantCulture),
                Quantity = Convert.ToDecimal(x.Quantity, CultureInfo.InvariantCulture),
                FirstBreakdownTradeId = x.FirstBreakdownTradeId,
                LastBreakdownTradeId = x.LastBreakdownTradeId,
                TradeTime = new DateTime(1970, 1, 1) + TimeSpan.FromMilliseconds(x.TradeTime),
                BuyerIsAMaker = x.BuyerIsAMaker
            };
        }
    }
}