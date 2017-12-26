using Newtonsoft.Json;
using System;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
#pragma warning disable 4014

namespace Trader.Host
{
    class Program
    {
        private static void Main(string[] args)
        {
            MainAsync().Wait();
        }

        private static async Task MainAsync()
        {
            //await DepthSockets();

            var reader = new OrdersReader();

            while (true)
            {
                Task.Run(async () =>
                {
                    var orders = await reader.Read("LTCUSDT");
                });


                await Task.Delay(5000);
            }
        }

        private static async Task DepthSockets()
        {
            using (var ws = new ClientWebSocket())
            {

                await ws.ConnectAsync(
                    new Uri("wss://stream.binance.com:9443/ws/ltcusdt@depth"),
                    CancellationToken.None);

                var buffer = new byte[1024];

                while (ws.State == WebSocketState.Open)
                {
                    WebSocketReceiveResult result;
                    var response = new StringBuilder(1024 * 4);

                    do
                    {
                        result = await ws.ReceiveAsync(
                            new ArraySegment<byte>(buffer),
                            CancellationToken.None);

                        response.Append(Encoding.UTF8.GetString(buffer));

                    } while (!result.EndOfMessage);

                    Console.WriteLine(response);
                }
            }
        }
    }
}