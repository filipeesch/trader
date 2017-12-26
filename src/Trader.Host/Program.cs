using Newtonsoft.Json;
using System;
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
            await DepthSockets();

            //var reader = new OrdersReader();

            //while (true)
            //{
            //    reader.Read("BTCUSDT", 100).ContinueWith(result =>
            //     {
            //         var orders = result.Result;

            //         var media = (orders.Asks[0].Price + orders.Bids[0].Price) / 2m;

            //         var intencaoDeCompra = orders.Bids.Sum(x => (100 / (media - x.Price)) * x.Quantity);
            //         var intencaoDeVenda = orders.Asks.Sum(x => (100 / (x.Price - media)) * x.Quantity);

            //         Console.WriteLine(intencaoDeCompra - intencaoDeVenda);
            //     });


            //    await Task.Delay(2000);
            //}
        }

        private static async Task DepthSockets()
        {
            var tradesListener = new WebSocketListener("wss://stream.binance.com:9443/ws/btcusdt@aggTrade");

            tradesListener.Listen<TradeEventRawResponse>(x =>
            {
                var response = TradeEventResponse.Parse(x);

                Console.WriteLine("Price: {0:N8}\t\tQtd: {1:N8}", response.Price, response.Quantity);
            });

            Console.ReadKey();
        }
    }
}