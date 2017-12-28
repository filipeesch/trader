using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Trader.Host.WebSocket.Messages
{
    [DataContract]
    public class OrdersBookRawResponse
    {
        [DataMember(Name = "lastUpdateId")]
        public long LastUpdateId { get; set; }

        [DataMember(Name = "bids")]
        public List<List<object>> Bids { get; set; }

        [DataMember(Name = "asks")]
        public List<List<object>> Asks { get; set; }
    }
}