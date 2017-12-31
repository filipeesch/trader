using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Trader.Host.HttpClients;
using Trader.Host.Services;
using Trader.Host.WebSocket.Core;
using Trader.Host.WebSocket.Listeners;
using Timer = Trader.Host.Helpers.Timer;

#pragma warning disable 4014

namespace Trader.Host
{
    internal static class Program
    {
        private static void Main()
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
        }

        private static async Task DepthSockets()
        {
            const string symbol = "btcusdt";

            var mediaAtual = new AverageCalculator(4);
            var mediaCurta = new AverageCalculator(6);
            var mediaLonga = new AverageCalculator(15);
            decimal valorAtual = 0;

            var saida = new StringBuilder();

            var comprado = false;
            var monitorandoPrecos = true;

            decimal relacaoUltimaMedia = 0;
            decimal maximaMedia = decimal.MinValue;
            decimal minimaMedia = decimal.MaxValue;

            OrderBookUpdateArgs bookUpate = null;

            await PreencherHistoricoMedias(symbol, mediaCurta, mediaLonga);

            Timer.RunEvery(1000, () =>
            {
                if (valorAtual == 0)
                    return;

                mediaAtual.AddSample(valorAtual);
            });

            Timer.RunEvery(60000, () =>
            {
                if (valorAtual == 0)
                    return;

                mediaLonga.AddSample(valorAtual);
            });

            Timer.RunEvery(5000, () =>
            {
                if (valorAtual == 0)
                    return;

                var ultimaMedia = mediaCurta.Calculate();
                mediaCurta.AddSample(valorAtual);
                relacaoUltimaMedia = (mediaCurta.Calculate() / ultimaMedia) * 100;
            });

            var sync = new object();

            decimal valorCompra = 0;

            Action print = () =>
            {
                lock (sync)
                {
                    Console.Clear();

                    var valorMediaLonga = mediaLonga.Calculate(valorAtual);
                    var valorMediaCurta = mediaCurta.Calculate(valorAtual);

                    Console.WriteLine("Longa({0}): {1:N8}\tCurta({2}): {3:N8}\nValor: {4:N8}\n",
                        mediaLonga.Count,
                        valorMediaLonga,
                        mediaCurta.Count,
                        valorMediaCurta,
                        valorAtual
                    );

                    maximaMedia = Math.Max(maximaMedia, valorMediaCurta);
                    minimaMedia = Math.Min(minimaMedia, valorMediaCurta);

                    var relacaoMediaMinima = (valorMediaCurta / minimaMedia) * 100;
                    var relacaoMediaMaxima = (valorMediaCurta / maximaMedia) * 100;

                    //foreach (var m in mediaCurta.Samples)
                    //    Console.Write("Curta: {0:N8}\t", m);

                    var relacaoMediaLonga = (valorMediaCurta / valorMediaLonga) * 100;

                    Console.WriteLine("\nRelação Medias: {0:N2}%\t\tRel. Ultima: {1:N2}%",
                        relacaoMediaLonga,
                        relacaoUltimaMedia);

                    Console.WriteLine("Relação Min: {0:N2}%\t\tRel. Max: {1:N2}%\n",
                        relacaoMediaMinima,
                        relacaoMediaMaxima);

                    //if (!monitorandoPrecos && relacaoMediaLonga <= 99.7m)
                    //{
                    //    saida.AppendFormat("{0:HH:mm:ss}:  ", DateTime.Now).AppendLine("Monitorando...");
                    //    monitorandoPrecos = true;
                    //}

                    if (monitorandoPrecos)
                    {
                        if (!comprado && relacaoMediaMinima >= 100.5m)
                        {
                            comprado = true;
                            saida
                                .AppendFormat("{0:HH:mm:ss}:  ", DateTime.Now)
                                .AppendFormat("Comprando a {0:N4}\n", valorCompra = valorAtual)
                                .AppendFormat("Relação Ultima {0:N2}\n", relacaoUltimaMedia);
                        }

                        if (comprado)
                        {
                            if (relacaoMediaMaxima <= 99.5m)
                            {
                                maximaMedia = decimal.MinValue;
                                minimaMedia = decimal.MaxValue;

                                comprado = false;
                                monitorandoPrecos = false;
                                saida
                                    .AppendFormat("{0:HH:mm:ss}:  ", DateTime.Now)
                                    .AppendFormat("Vendendo a {0:N4}\n", valorAtual)
                                    .AppendFormat("Lucro: {0:N4} ( {1:N2}% )\n", valorAtual - valorCompra, (valorAtual / valorCompra) * 100)
                                    .AppendFormat("Relação Ultima {0:N2}\n\n", relacaoUltimaMedia);
                            }

                            var relacaoMediaAtual = (mediaAtual.Calculate(valorAtual) / valorMediaCurta) / 100;

                            if (relacaoMediaAtual <= 99.5m)
                            {
                                maximaMedia = decimal.MinValue;
                                minimaMedia = decimal.MaxValue;

                                comprado = false;
                                monitorandoPrecos = false;
                                saida
                                    .AppendFormat("{0:HH:mm:ss}:  ", DateTime.Now)
                                    .AppendFormat("Vendendo Stop-Loss a {0:N8}\n", valorAtual)
                                    .AppendFormat("Prejuizo: {0:N4} ( {1:N2}% )\n", valorAtual - valorCompra, (valorAtual / valorCompra) * 100)
                                    .AppendFormat("Relação Ultima {0:N2}\n\n", relacaoMediaAtual);
                            }
                        }
                    }

                    var saidaRaw = saida.ToString();
                    Console.WriteLine(saidaRaw);
                    File.WriteAllText("saida.txt", saidaRaw);

                    //if (bookUpate == null)
                    //    return;

                    //Console.WriteLine("\n");

                    //var limit = new[] { 20, bookUpate.Bids.Count(), bookUpate.Asks.Count() }.Min();

                    //for (var i = 0; i < limit; ++i)
                    //{
                    //    Console.WriteLine("Preço: {0:N4}\tQtd: {1:N4}\t\t\t{2:N4}\tQtd: {3:N4}",
                    //        bookUpate.Bids.ElementAt(i).Price,
                    //        bookUpate.Bids.ElementAt(i).Quantity,
                    //        bookUpate.Asks.ElementAt(i).Price,
                    //        bookUpate.Asks.ElementAt(i).Quantity);
                    //}
                }
            };


            using (var binanceSocket = new BinanceWebSocket())
            {
                //var book = new OrderBook(binanceSocket, symbol);


                binanceSocket.Register(new AggregatedTradeEventListener(symbol, response =>
                {
                    valorAtual = response.Price;
                    print();
                }));


                //book.OnUpdate(x =>
                //{
                //    bookUpate = x;
                //    print();
                //});


                await binanceSocket.Start();
                //await book.Start();

                Console.ReadKey();
            }
        }

        private static async Task PreencherHistoricoMedias(
            string symbol,
            AverageCalculator mediaCurta,
            AverageCalculator mediaLonga)
        {
            var client = new BinanceOrderClient();

            var klinesResponse = await client.Klines(symbol, "1m", 25);

            foreach (var kline in klinesResponse.Values)
            {
                mediaCurta.AddSample(kline.Close);
                mediaLonga.AddSample(kline.Close);
            }
        }
    }
}