using System.Runtime.Serialization;

namespace Trader.Host.Messages
{
    [DataContract]
    public class AccountInfoBalanceMessage
    {
        [DataMember(Name = "a")]
        public string Asset { get; set; }

        [DataMember(Name = "f")]
        public decimal FreeAmmount { get; set; }

        [DataMember(Name = "l")]
        public decimal LockedAmmount { get; set; }
    }
}