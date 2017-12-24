using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Trader.Host
{
    class Program
    {
        private const string Host = "https://api.binance.com";

        private static void Main(string[] args)
        {
            MainAsync().Wait();
        }

        private static async Task MainAsync()
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

                    do
                    {
                        result = await ws.ReceiveAsync(
                            new ArraySegment<byte>(buffer),
                            CancellationToken.None);
                    } while (!result.EndOfMessage);


                    var data = Encoding.UTF8.GetString(buffer);

                    Console.WriteLine(data);
                }

                //var http = new HttpClient();

                //var response = http.GetAsync(Host + "/api/v1/depth?symbol=LTCUSDT").Result.Content.ReadAsStringAsync()
                //.Result;

                //var obj = JsonConvert.DeserializeObject<OrderBookResponse>(response);
            }
        }

        [DataContract]
        public class OrderBookResponse
        {
            [DataMember(Name = "lastUpdateId")] public long LastUpdateId { get; set; }

            [DataMember(Name = "bids")] public List<List<object>> Bids { get; set; }

            [DataMember(Name = "asks")] public List<List<object>> Asks { get; set; }
        }
    }
}