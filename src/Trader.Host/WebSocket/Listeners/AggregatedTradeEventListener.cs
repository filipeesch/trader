using System;
using Newtonsoft.Json.Linq;
using Trader.Host.Messages;

namespace Trader.Host.WebSocket.Listeners
{
    public class AggregatedTradeEventListener : IBinanceEventListener
    {
        public AggregatedTradeEventListener(string symbol, Action<TradeEventResponse> action)
        {
            Symbol = symbol;
            Action = action;
        }

        public string Symbol { get; }
        public Action<TradeEventResponse> Action { get; }

        public string EventUri => Symbol + "@aggTrade";

        public void RawAction(JObject json)
        {
            var message = json.ToObject<TradeEventRawResponse>();

            if (message.Symbol.Equals(Symbol, StringComparison.OrdinalIgnoreCase))
                Action(TradeEventResponse.Parse(message));
        }
    }
}