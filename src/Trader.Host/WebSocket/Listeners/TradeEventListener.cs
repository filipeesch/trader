using System;
using Newtonsoft.Json.Linq;
using Trader.Host.Messages;

namespace Trader.Host.WebSocket.Listeners
{
    public class TradeEventListener : IBinanceEventListener
    {
        public TradeEventListener(string symbol, Action<TradeEventResponse> action)
        {
            Symbol = symbol;
            Action = action;
        }

        public string Symbol { get; }
        public Action<TradeEventResponse> Action { get; }

        public string EventUri => Symbol + "@trade";

        public void RawAction(JObject json)
        {
            //throw new NotImplementedException();
        }
    }
}