using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Trader.Host.HttpOperations;
using Trader.Host.WebSocket.Core;
using Trader.Host.WebSocket.Listeners;

namespace Trader.Host
{
    class Program
    {
        private static void Main(string[] args)
        {
            ServicePointManager.ServerCertificateValidationCallback = (s, certificate, chain, sslPolicyErrors) => true;

            MainAsync().Wait();
        }

        private static async Task MainAsync()
        {
            //var op = new OrderOperations();

            //await op.NewOrder(new NewOrderRequest
            //{
            //    OrderId = "teste1",
            //    Side = OrderSide.Buy,
            //    Symbol = "ltcusdt",
            //    Type = OrderType.Stop,
            //    Date = DateTime.UtcNow,
            //    StopPrice = 99,
            //    Price = 100,
            //    Quantity = 0.1m
            //});

            //await op.CancelOrder(new CancelOrderRequest
            //{
            //    OrigOrderId = "teste1",
            //    Symbol = "ltcusdt",
            //    Date = DateTime.UtcNow
            //});

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
            const string symbol = "btcusdt";

            var mediaPreco = new Queue<decimal>(16);

            using (var binanceSocket = new BinanceWebSocket())
            {
                var book = new OrderBook(binanceSocket, symbol);


                //binanceSocket.Register(new AggregatedTradeEventListener(symbol, response =>
                //{
                //    //Console.WriteLine("BTC -> USDT\tPrice: {0:N8}\t\tQtd: {1:N8}", response.Price, response.Quantity);

                //    mediaPreco.Enqueue(response.Price);

                //    if (mediaPreco.Count > 5)
                //    {
                //        mediaPreco.Dequeue();

                //        Console.WriteLine("Preço médio: {0:N8}\tPreço: {1:N8}",
                //            Math.Round(mediaPreco.Average(), 8), response.Price);
                //    }
                //}));


                book.OnUpdate(x =>
                {
                    Console.Clear();

                    for (var i = 0; i < 20; ++i)
                    {
                        Console.WriteLine("Preço: {0:N4}\tQtd: {1:N4}\t\t\t{2:N4}\tQtd: {3:N4}",
                            x.Bids.ElementAt(i).Price,
                            x.Bids.ElementAt(i).Quantity,
                            x.Asks.ElementAt(i).Price,
                            x.Asks.ElementAt(i).Quantity);
                    }
                });


                await binanceSocket.Start();
                await book.Start();

                Console.ReadKey();
            }
        }
    }
}