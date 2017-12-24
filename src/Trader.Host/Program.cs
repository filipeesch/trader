using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Trader.Host
{
    class Program
    {
        const string Host = "https://api.binance.com";

        static void Main(string[] args)
        {
            var http = new HttpClient();

            var response = http.GetAsync(Host + "/api/v1/depth?symbol=LTCUSDT").Result.Content.ReadAsStringAsync().Result;

            var obj = JsonConvert.DeserializeObject<OrderBookResponse>(response);
        }
    }

    [DataContract]
    public class OrderBookResponse
    {
        [DataMember(Name = "lastUpdateId")]
        public long LastUpdateId { get; set; }

        [DataMember(Name = "bids")]
        public List<List<object>> Bids { get; set; }

        [DataMember(Name = "asks")]
        public List<List<object>> Asks { get; set; }
    }
}
