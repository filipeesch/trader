//using System;
//using Newtonsoft.Json.Linq;
//using Trader.Host.Messages;

//namespace Trader.Host.WebSocket.Listeners
//{
//    public class KlineEventListener : IBinanceEventListener
//    {
//        private readonly string _interval;

//        public KlineEventListener(string symbol, string interval, Action<KlinesEventResponse> action)
//        {
//            _interval = interval;
//            Symbol = symbol;
//            Action = action;
//        }

//        public string Symbol { get; }
//        public Action<KlinesEventResponse> Action { get; }

//        public string EventUri => Symbol + "@klines_" + _interval;

//        public void RawAction(JObject json)
//        {
//            var message = json.ToObject<TradeEventRawResponse>();

//            if (message.Symbol.Equals(Symbol, StringComparison.OrdinalIgnoreCase))
//                Action(KlinesEventResponse.Parse(message));
//        }
//    }

//    public class KlinesEventResponse
//    {
//    }

//    /*
//{
//  "e": "kline",     // Event type
//  "E": 123456789,   // Event time
//  "s": "BNBBTC",    // Symbol
//  "k": {
//    "t": 123400000, // Kline start time
//    "T": 123460000, // Kline close time
//    "s": "BNBBTC",  // Symbol
//    "i": "1m",      // Interval
//    "f": 100,       // First trade ID
//    "L": 200,       // Last trade ID
//    "o": "0.0010",  // Open price
//    "c": "0.0020",  // Close price
//    "h": "0.0025",  // High price
//    "l": "0.0015",  // Low price
//    "v": "1000",    // Base asset volume
//    "n": 100,       // Number of trades
//    "x": false,     // Is this kline closed?
//    "q": "1.0000",  // Quote asset volume
//    "V": "500",     // Taker buy base asset volume
//    "Q": "0.500",   // Taker buy quote asset volume
//    "B": "123456"   // Ignore
//  }
//}
//     */
//}