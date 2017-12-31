using System.Runtime.Serialization;

namespace Trader.Host.Messages
{
    [DataContract]
    public class TradeEventRawResponse
    {
        [DataMember(Name = "e")]
        public string EventType { get; set; }

        [DataMember(Name = "E")]
        public long EventTime { get; set; }

        [DataMember(Name = "s")]
        public string Symbol { get; set; }

        [DataMember(Name = "a")]
        public int AggregatedTradeId { get; set; }

        [DataMember(Name = "p")]
        public string Price { get; set; }

        [DataMember(Name = "q")]
        public string Quantity { get; set; }

        [DataMember(Name = "f")]
        public int FirstBreakdownTradeId { get; set; }

        [DataMember(Name = "l")]
        public int LastBreakdownTradeId { get; set; }

        [DataMember(Name = "T")]
        public long TradeTime { get; set; }

        [DataMember(Name = "m")]
        public bool BuyerIsAMaker { get; set; }
    }
}