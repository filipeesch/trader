using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Trader.Host.Helpers;
using Trader.Host.Messages;

namespace Trader.Host.HttpOperations
{
    public class UserAccountOperations
    {
        private readonly byte[] _apiSecret;
        private readonly string _apiKey;
        private readonly string _apiBaseAddress;

        public UserAccountOperations()
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


        public async Task<AccountInfoResponse> AccountInfo()
        {
            var http = new HttpClient();

            http.DefaultRequestHeaders.Add("X-MBX-APIKEY", _apiKey);

            var body = "timestamp=" + DateHelpers.ToBinanceDate(DateTime.UtcNow);
            body += "&signature=" + CreateSignature(body);

            var response = await http.GetAsync(
                _apiBaseAddress + "/api/v3/account?" + body);

            var result = await response.Content.ReadAsStringAsync();

            if (response.StatusCode != HttpStatusCode.OK)
                return null;

            var data = JsonConvert.DeserializeObject<AccountInfoResponse>(result);

            return data;
        }


        public async Task<string> CreateListenKey()
        {
            var http = new HttpClient();

            http.DefaultRequestHeaders.Add("X-MBX-APIKEY", _apiKey);

            var response = await http.PostAsync(
                _apiBaseAddress + "/api/v1/userDataStream",
                new StringContent(string.Empty));

            if (response.StatusCode != HttpStatusCode.OK)
                return string.Empty;

            var responseContent = await response.Content.ReadAsStringAsync();

            var responseJson = JObject.Parse(responseContent);

            return responseJson["listenKey"].Value<string>();
        }


        public async Task<bool> PingListenKey(string listenKey)
        {
            var http = new HttpClient();

            http.DefaultRequestHeaders.Add("X-MBX-APIKEY", _apiKey);

            var response = await http.PutAsync(
                _apiBaseAddress + "/api/v1/userDataStream",
                new FormUrlEncodedContent(
                    new[]
                    {
                        new KeyValuePair<string, string>("listenKey", listenKey),
                    }));

            await response.Content.ReadAsStringAsync();


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
