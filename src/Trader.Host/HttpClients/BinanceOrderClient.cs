using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Trader.Host.WebSocket.Messages;

namespace Trader.Host.HttpClients
{
    public class BinanceOrderClient
    {
        private const string Host = "https://api.binance.com";

        public async Task<OrdersBookResponse> Depth(string symbol, int limit = 100)
        {
            var http = new HttpClient();

            using (var response = await http.GetAsync($"{Host}/api/v1/depth?symbol={symbol}&limit={limit}"))
            {
                var responseText = await response.Content.ReadAsStringAsync();

                return OrdersBookResponse.Parse(JsonConvert.DeserializeObject<OrdersBookRawResponse>(responseText));
            }
        }
    }
}