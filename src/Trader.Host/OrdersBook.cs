using System.Collections.Generic;

namespace Trader.Host
{
    public class OrdersBook
    {
        public OrdersBook()
        {
            Bids = new List<Order>(100);
            Asks = new List<Order>(100);
        }

        public long LastUpdateId { get; set; }

        public List<Order> Bids { get; set; }

        public List<Order> Asks { get; set; }
    }
}