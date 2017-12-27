﻿using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

#pragma warning disable 4014

namespace Trader.Host.WebSocket.Core
{
    public class WebSocketListener : IDisposable
    {
        private readonly ClientWebSocket _client;
        private bool _running = false;

        public WebSocketListener()
        {
            _client = new ClientWebSocket();
        }

        public void OnMessage(Action<string> handler)
        {
            Task.Run(async () =>
            {
                while (_running)
                {
                    var response = await ReadResponse();

                    handler(response.ToString());
                }
            });
        }

        public void OnMessage<T>(Action<T> handler)
        {
            OnMessage(response => handler(JsonConvert.DeserializeObject<T>(response)));
        }

        public async Task Connect(string uri)
        {
            if (_running)
                return;

            _running = true;

            await _client.ConnectAsync(new Uri(uri), CancellationToken.None);

            if (_client.State != WebSocketState.Open)
            {
                _running = false;
                throw new Exception($"Não foi possível conectar no endereço '{uri}'");
            }
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