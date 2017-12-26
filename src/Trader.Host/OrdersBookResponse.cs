using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Trader.Host
{
    [DataContract]
    public class OrdersBookResponse
    {
        [DataMember(Name = "lastUpdateId")]
        public long LastUpdateId { get; set; }

        [DataMember(Name = "bids")]
        public List<List<object>> Bids { get; set; }

        [DataMember(Name = "asks")]
        public List<List<object>> Asks { get; set; }
    }
}