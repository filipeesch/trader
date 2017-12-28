using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Trader.Host.HttpClients;
using Trader.Host.WebSocket.Core;
using Trader.Host.WebSocket.Messages;

namespace Trader.Host
{
    public class OrderBook
    {
        private readonly string _symbol;

        private readonly Dictionary<decimal, Order> _bids = new Dictionary<decimal, Order>(1000);
        private readonly Dictionary<decimal, Order> _asks = new Dictionary<decimal, Order>(1000);

        private readonly ObservableCollection<OrdersBookMessage> _messages =
            new ObservableCollection<OrdersBookMessage>();

        private readonly BinanceOrderClient _orderClient = new BinanceOrderClient();

        private Action<OrderBookUpdateArgs> _hanlder;

        public OrderBook(BinanceWebSocket binanceSocket, string symbol)
        {
            _symbol = symbol;

            binanceSocket.Register(new DiffDepthEventListener(_symbol, message => _messages.Add(message)));
        }

        public async Task Start()
        {
            var book = await _orderClient.Depth(_symbol, 1000);

            book.Asks.ForEach(x => _asks.Add(x.Price, x));
            book.Bids.ForEach(x => _bids.Add(x.Price, x));

            ProcessMessages(book);
        }

        private void ProcessMessages(OrdersBookResponse book)
        {
            var messages = _messages.Where(x => x.FinalUpdateId > book.LastUpdateId).ToList();
            _messages.Clear();

            var nextUpdateId = book.LastUpdateId + 1;

            while (true)
            {
                var message = messages.FirstOrDefault(x => x.FirstUpdateId == nextUpdateId);

                if (message == null)
                    break;

                messages.Remove(message);

                nextUpdateId = message.FinalUpdateId + 1;

                UpdateOrders(_bids, message.Bids);
                UpdateOrders(_asks, message.Asks);
            }

            _hanlder(new OrderBookUpdateArgs
            {
                Bids = _bids.Values,
                Asks = _asks.Values
            });

            /*
             Drop any event where u is <= lastUpdateId in the snapshot
            The first processed should have U <= lastUpdateId+1 AND u >= lastUpdateId+1
            While listening to the stream, each new event's U should be equal to the previous event's u+1
             */
        }

        private void UpdateOrders(IDictionary<decimal, Order> oldOrders, IEnumerable<Order> newOrders)
        {
            foreach (var newOrder in newOrders)
            {
                if (newOrder.Quantity == 0m)
                    oldOrders.Remove(newOrder.Price);

                else if (oldOrders.TryGetValue(newOrder.Price, out var oldOrder))
                {
                    oldOrder.Quantity = newOrder.Quantity;
                }
            }
        }

        public void OnUpdate(Action<OrderBookUpdateArgs> handler)
        {
            _hanlder = handler;
        }

        public class OrderBookUpdateArgs
        {
            public IEnumerable<Order> Asks { get; set; }
            public IEnumerable<Order> Bids { get; set; }
        }
    }
}