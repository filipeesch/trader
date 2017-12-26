using System;
using System.Text;
using Trader.Host.Helpers;

namespace Trader.Host.HttpOperations
{
    public class CancelOrderRequest
    {
        public string Symbol { get; set; }
        public string OrigOrderId { get; set; }
        public long? ReceiveWindow { get; set; }
        public DateTime Date { get; set; }

        public string Serialize()
        {
            var result = new StringBuilder(128);

            result.AppendFormat(
                "symbol={0}&origClientOrderId={1}&timestamp={2}",
                Symbol.ToUpper(),
                OrigOrderId,
                DateHelpers.ToBinanceDate(Date)
            );

            if (ReceiveWindow.HasValue)
                result.Append("&recvWindow=").Append(ReceiveWindow.Value);

            return result.ToString();
        }
    }
}