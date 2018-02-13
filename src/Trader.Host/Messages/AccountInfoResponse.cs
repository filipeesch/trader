using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Trader.Host.Messages
{
    [DataContract]
    public class AccountInfoResponse
    {
        [DataMember(Name = "makerCommission")]
        public decimal MakerComissionRate { get; set; }

        [DataMember(Name = "takerCommission")]
        public decimal TakerComissionRate { get; set; }

        [DataMember(Name = "buyerCommission")]
        public decimal BuyerComissionRate { get; set; }

        [DataMember(Name = "sellerCommission")]
        public decimal SellerComissionRate { get; set; }

        [DataMember(Name = "canTrade")]
        public bool CanTrade { get; set; }

        [DataMember(Name = "canWithdraw")]
        public bool CanWithdraw { get; set; }

        [DataMember(Name = "canDeposit")]
        public bool CanDeposit { get; set; }

        [DataMember(Name = "updateTime")]
        public long UpdateTime { get; set; }

        [DataMember(Name = "balances")]
        public List<AccountInfoBalanceResponse> Balances { get; set; }
    }
}