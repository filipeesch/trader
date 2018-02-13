using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Trader.Host.Messages
{
    [DataContract]
    public class OnAccountInfoMessage
    {
        [DataMember(Name = "e")]
        public string EventType { get; set; }

        [DataMember(Name = "E")]
        public long EventTime { get; set; }

        [DataMember(Name = "m")]
        public decimal MakerComissionRate { get; set; }

        [DataMember(Name = "t")]
        public decimal TakerComissionRate { get; set; }

        [DataMember(Name = "b")]
        public decimal BuyerComissionRate { get; set; }

        [DataMember(Name = "s")]
        public decimal SellerComissionRate { get; set; }

        [DataMember(Name = "T")]
        public bool CanTrade { get; set; }

        [DataMember(Name = "W")]
        public bool CanWithdraw { get; set; }

        [DataMember(Name = "D")]
        public bool CanDeposit { get; set; }

        [DataMember(Name = "u")]
        public long LastEventTime { get; set; }

        [DataMember(Name = "B")]
        public List<AccountInfoBalanceMessage> Balances { get; set; }
    }
}