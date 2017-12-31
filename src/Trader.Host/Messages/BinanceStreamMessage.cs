using System.Runtime.Serialization;
using Newtonsoft.Json.Linq;

namespace Trader.Host.Messages
{
    [DataContract]
    public class BinanceStreamMessage
    {
        [DataMember(Name = "stream")]
        public string Stream { get; set; }

        [DataMember(Name = "data")]
        public JObject Data { get; set; }
    }
}