using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Trader.Host.Messages;

namespace Trader.Host.HttpClients
{
    public class BinanceOrderClient
    {
        private const string Host = "https://api.binance.com";

        public async Task<OrdersBookResponse> Depth(string symbol, int limit = 100)
        {
            var http = new HttpClient();

            using (var response = await http.GetAsync($"{Host}/api/v1/depth?symbol={symbol.ToUpper()}&limit={limit}"))
            {
                var responseText = await response.Content.ReadAsStringAsync();

                return OrdersBookResponse.Parse(JsonConvert.DeserializeObject<OrdersBookRawResponse>(responseText));
            }
        }

        public async Task<KlinesResponse> Klines(string symbol, string interval = "1m", int limit = 500)
        {
            var http = new HttpClient();

            var url = $"{Host}/api/v1/klines?symbol={symbol.ToUpper()}&interval={interval}&limit={limit}";

            using (var response = await http.GetAsync(url))
            {
                var responseText = await response.Content.ReadAsStringAsync();

                return KlinesResponse.Parse(JsonConvert.DeserializeObject<List<List<object>>>(responseText));
            }
        }
    }
}