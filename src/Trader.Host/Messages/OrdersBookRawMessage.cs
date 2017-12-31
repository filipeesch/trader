using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Trader.Host.Messages
{
    [DataContract]
    public class OrdersBookRawMessage
    {
        [DataMember(Name = "e")]
        public string EventType { get; set; }

        [DataMember(Name = "E")]
        public long EventTime { get; set; }

        [DataMember(Name = "s")]
        public string Symbol { get; set; }

        [DataMember(Name = "U")]
        public long FirsttUpdateId { get; set; }

        [DataMember(Name = "u")]
        public long FinalUpdateId { get; set; }

        [DataMember(Name = "b")]
        public List<List<object>> Bids { get; set; }

        [DataMember(Name = "a")]
        public List<List<object>> Asks { get; set; }
    }

    /*
     {
  "e": "depthUpdate", // Event type
  "E": 123456789,     // Event time
  "s": "BNBBTC",      // Symbol
  "U": 157,           // First update ID in event
  "u": 160,           // Final update ID in event
  "b": [              // Bids to be updated
    [
      "0.0024",       // price level to be updated
      "10",
      []              // ignore
    ]
  ],
  "a": [              // Asks to be updated
    [
      "0.0026",       // price level to be updated
      "100",          // quantity
      []              // ignore
    ]
  ]
}
     */
}