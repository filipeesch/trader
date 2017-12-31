using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Trader.Host.ValueObjects;

namespace Trader.Host.Messages
{
    public class OrdersBookResponse
    {
        public OrdersBookResponse()
        {
            Bids = new List<Order>(100);
            Asks = new List<Order>(100);
        }

        public long LastUpdateId { get; private set; }

        public List<Order> Bids { get; private set; }

        public List<Order> Asks { get; private set; }

        public static OrdersBookResponse Parse(OrdersBookRawResponse data)
        {
            var orders = new OrdersBookResponse
            {
                LastUpdateId = data.LastUpdateId,
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