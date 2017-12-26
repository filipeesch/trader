using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Trader.Host.HttpOperations
{
    public class OrderOperations
    {
        private readonly byte[] _apiSecret;
        private readonly string _apiKey;
        private readonly string _apiBaseAddress;

        public OrderOperations()
        {
            _apiBaseAddress = ConfigurationManager.AppSettings["ApiBaseAddress"];
            _apiKey = ConfigurationManager.AppSettings["ApiKey"];
            _apiSecret = Encoding.UTF8.GetBytes(ConfigurationManager.AppSettings["ApiSecret"]);
        }

        public async Task<bool> NewOrder(NewOrderRequest request)
        {
            var http = new HttpClient();

            http.DefaultRequestHeaders.Add("X-MBX-APIKEY", _apiKey);

            var body = request.Serialize();
            body += "&signature=" + CreateSignature(body);

            var response = await http.PostAsync(
                _apiBaseAddress + "/api/v3/order",
                new StringContent(body)
            );

            var result = await response.Content.ReadAsStringAsync();

            return response.StatusCode == HttpStatusCode.OK;
        }


        public async Task<bool> CancelOrder(CancelOrderRequest request)
        {
            var http = new HttpClient();

            http.DefaultRequestHeaders.Add("X-MBX-APIKEY", _apiKey);

            var body = request.Serialize();
            body += "&signature=" + CreateSignature(body);

            var response = await http.DeleteAsync(
                _apiBaseAddress + "/api/v3/order?" + body);

            var result = await response.Content.ReadAsStringAsync();

            return response.StatusCode == HttpStatusCode.OK;
        }

        private string CreateSignature(string data)
        {
            using (var hmac = new HMACSHA256(_apiSecret))
            {
                var hashValue = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));

                return string.Concat(hashValue.Select(x => x.ToString("x2")));
            }
        }
    }
}
