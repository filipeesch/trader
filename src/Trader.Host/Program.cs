using System;
using System.Globalization;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Trader.Host.HttpOperations;
using Trader.Host.Listeners;

#pragma warning disable 4014

namespace Trader.Host
{
    class Program
    {
        private static void Main(string[] args)
        {
            ServicePointManager.ServerCertificateValidationCallback =
                delegate (object s, X509Certificate certificate,
                    X509Chain chain, SslPolicyErrors sslPolicyErrors)
                { return true; };

            MainAsync().Wait();
        }

        private static async Task MainAsync()
        {
            var op = new OrderOperations();

            await op.NewOrder(new NewOrderRequest
            {
                OrderId = "teste1",
                Side = OrderSide.Buy,
                Symbol = "ltcusdt",
                Type = OrderType.Stop,
                Date = DateTime.UtcNow,
                StopPrice = 99,
                Price = 100,
                Quantity = 0.1m
            });

            await op.CancelOrder(new CancelOrderRequest
            {
                OrigOrderId = "teste1",
                Symbol = "ltcusdt",
                Date = DateTime.UtcNow
            });

            //await DepthSockets();

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
            using (var tradesListener = new CoinTradeListener("btcusdt"))
            {
                tradesListener.Listen(response =>
                {
                    Console.WriteLine("Price: {0:N8}\t\tQtd: {1:N8}", response.Price, response.Quantity);
                });

                Console.ReadKey();

                tradesListener.Dispose();
            }
        }
    }
}