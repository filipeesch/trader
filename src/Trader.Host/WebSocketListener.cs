using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

#pragma warning disable 4014

namespace Trader.Host
{
    public class WebSocketListener : IDisposable
    {
        private readonly string _uri;
        private readonly ClientWebSocket _client;
        private bool _running = false;

        public WebSocketListener(string uri)
        {
            _uri = uri;
            _client = new ClientWebSocket();
        }

        public async Task Listen(Action<string> handler)
        {
            if (_running)
                return;

            _running = true;

            await Connect();

            Task.Run(async () =>
            {
                while (_running)
                {
                    if (_client.State != WebSocketState.Open)
                        await Connect();

                    var response = await ReadResponse();

                    handler(response.ToString());
                }
            });
        }

        public async Task Listen<T>(Action<T> handler)
        {
            await Listen(response => handler(JsonConvert.DeserializeObject<T>(response)));
        }

        private async Task Connect()
        {
            await _client.ConnectAsync(new Uri(_uri), CancellationToken.None);

            if (_client.State != WebSocketState.Open)
                throw new Exception($"Não foi possível conectar no endereço '{_uri}'");
        }

        private async Task<StringBuilder> ReadResponse()
        {
            var buffer = new byte[1024];

            WebSocketReceiveResult result;
            var response = new StringBuilder(1024 * 4);

            do
            {
                result = await _client.ReceiveAsync(
                    new ArraySegment<byte>(buffer),
                    CancellationToken.None);

                response.Append(Encoding.UTF8.GetString(buffer));
            } while (!result.EndOfMessage);

            return response;
        }

        public void Dispose()
        {
            _running = false;
            _client.Dispose();
        }
    }
}
