using System;
using System.Configuration;
using System.Threading.Tasks;

namespace Trader.Host.Listeners
{
    public class CoinTradeListener : IDisposable
    {
        private readonly WebSocketListener _socketListener;

        public CoinTradeListener(string sybmbol)
        {
            var baseAddress = ConfigurationManager.AppSettings["WebSocketsBaseAddress"];

            _socketListener = new WebSocketListener($"{baseAddress}{sybmbol}@aggTrade");
        }

        public Task Listen(Action<TradeEventResponse> handler)
        {
            return _socketListener.Listen<TradeEventRawResponse>(x =>
            {
                var response = TradeEventResponse.Parse(x);

                handler(response);
            });
        }

        public void Dispose()
        {
            _socketListener.Dispose();
        }
    }
}
