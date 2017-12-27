using Newtonsoft.Json.Linq;

namespace Trader.Host.WebSocket.Listeners
{
    public interface IBinanceEventListener
    {
        string EventUri { get; }
        void RawAction(JObject message);
    }
}