using System;
using Newtonsoft.Json.Linq;
using Trader.Host.Messages;

namespace Trader.Host.WebSocket.Listeners
{
    public class DiffDepthEventListener : IBinanceEventListener
    {
        public string Symbol { get; }
        public Action<OrdersBookMessage> Action { get; }
        public string EventUri => Symbol + "@depth";

        public DiffDepthEventListener(string symbol, Action<OrdersBookMessage> action)
        {
            Symbol = symbol;
            Action = action;
        }

        public void RawAction(JObject message)
        {
            var tmp = message.ToObject<OrdersBookRawMessage>();

            Action(OrdersBookMessage.Parse(tmp));
        }
    }
}