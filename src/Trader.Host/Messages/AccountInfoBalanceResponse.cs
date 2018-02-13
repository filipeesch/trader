using System.Runtime.Serialization;

namespace Trader.Host.Messages
{
    [DataContract]
    public class AccountInfoBalanceResponse
    {
        [DataMember(Name = "asset")]
        public string Asset { get; set; }

        [DataMember(Name = "free")]
        public decimal FreeAmmount { get; set; }

        [DataMember(Name = "locked")]
        public decimal LockedAmmount { get; set; }
    }
}