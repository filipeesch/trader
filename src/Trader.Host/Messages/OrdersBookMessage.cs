using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Trader.Host.Helpers;
using Trader.Host.ValueObjects;

namespace Trader.Host.Messages
{
    public class OrdersBookMessage
    {
        public OrdersBookMessage()
        {
            Bids = new List<Order>(100);
            Asks = new List<Order>(100);
        }

        public string EventType { get; private set; }

        public DateTime EventTime { get; private set; }

        public string Symbol { get; private set; }

        public long FirstUpdateId { get; private set; }

        public long FinalUpdateId { get; private set; }

        public List<Order> Bids { get; private set; }

        public List<Order> Asks { get; private set; }

        public static OrdersBookMessage Parse(OrdersBookRawMessage data)
        {
            var orders = new OrdersBookMessage()
            {
                EventType = data.EventType,
                EventTime = DateHelpers.FromBinanceDate(data.EventTime),
                Symbol = data.Symbol,
                FirstUpdateId = data.FirsttUpdateId,
                FinalUpdateId = data.FinalUpdateId,
                Bids = data.Bids.Select(x => new Order
                {
                    Price = Convert.ToDecimal(x[0], CultureInfo.InvariantCulture),
                    Quantity = Convert.ToDecimal(x[1], CultureInfo.InvariantCulture)
                }).ToList(),
                Asks = data.Asks.Select(x => new Order
                {
                    Price = Convert.ToDecimal(x[0], CultureInfo.InvariantCulture),
                    Quantity = Convert.ToDecimal(x[1], CultureInfo.InvariantCulture)
                }).ToList()
            };

            return orders;
        }
    }
}