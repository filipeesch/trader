using System;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Trader.Host
{
    public class OrdersReader
    {
        private const string Host = "https://api.binance.com";

        public async Task<OrdersBook> Read(string symbol, int limit = 100)
        {
            var http = new HttpClient();

            using (var response = await http.GetAsync($"{Host}/api/v1/depth?symbol={symbol}&limit={limit}"))
            {
                var responseText = await response.Content.ReadAsStringAsync();

                return MapDepthResponse(JsonConvert.DeserializeObject<OrdersBookResponse>(responseText));
            }
        }

        private OrdersBook MapDepthResponse(OrdersBookResponse data)
        {
            var orders = new OrdersBook
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