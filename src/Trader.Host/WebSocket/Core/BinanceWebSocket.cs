using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Trader.Host.Helpers;
using Trader.Host.WebSocket.Listeners;
using Trader.Host.WebSocket.Messages;

namespace Trader.Host.WebSocket.Core
{
    public class BinanceWebSocket : IDisposable
    {
        private readonly WebSocketListener _webSocket;

        private readonly ConcurrentBag<IBinanceEventListener> _listeners = new ConcurrentBag<IBinanceEventListener>();

        public BinanceWebSocket()
        {
            _webSocket = new WebSocketListener();
        }

        public void Register(IBinanceEventListener listener)
        {
            _listeners.Add(listener);
        }

        private void OnMessageArrives(string rawMessage)
        {
            var message = JsonConvert.DeserializeObject<BinanceStreamMessage>(rawMessage);

            _listeners
                .Where(x => x.EventUri == message.Stream)
                .ForEach(x => Task.Run(() => x.RawAction(message.Data)));
        }

        public async Task Start()
        {
            var uri = new StringBuilder(256);

            var baseAddress = ConfigurationManager.AppSettings["WebSocketsBaseAddress"];

            uri.Append(baseAddress).Append("/stream?streams=");

            foreach (var listener in _listeners)
                uri.Append(listener.EventUri).Append("/");

            await _webSocket.Connect(uri.ToString().TrimEnd('/'));

            _webSocket.OnMessage(OnMessageArrives);
        }

        public void Dispose()
        {
            _webSocket.Dispose();
        }
    }
}