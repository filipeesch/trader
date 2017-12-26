using System;
using System.Globalization;
using System.Text;
using Trader.Host.Helpers;

namespace Trader.Host.HttpOperations
{
    public class NewOrderRequest
    {
        public NewOrderRequest()
        {
            TimeInForce = "GTC";
        }

        public string Symbol { get; set; }
        public OrderSide Side { get; set; }
        public OrderType Type { get; set; }
        public string TimeInForce { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public string OrderId { get; set; }
        public decimal? StopPrice { get; set; }
        public long? ReceiveWindow { get; set; }
        public DateTime Date { get; set; }

        public string Serialize()
        {
            var result = new StringBuilder(128);

            result.AppendFormat(
                "symbol={0}&side={1}&type={2}&timeInForce={3}&quantity={4}&price={5}&timestamp={6}",
                Symbol.ToUpper(),
                Side == OrderSide.Buy ? "BUY" : "SELL",
                Type == OrderType.Limit ? "LIMIT" : (Side == OrderSide.Buy ? "TAKE_PROFIT_LIMIT" : "STOP_LOSS_LIMIT"),
                TimeInForce,
                Quantity.ToString("N8", CultureInfo.InvariantCulture),
                Price.ToString("N8", CultureInfo.InvariantCulture),
                DateHelpers.ToBinanceDate(Date)
            );

            if (!string.IsNullOrEmpty(OrderId))
                result.Append("&newClientOrderId=").Append(OrderId);

            if (StopPrice.HasValue)
                result.Append("&stopPrice=").Append(StopPrice.Value.ToString("N2", CultureInfo.InvariantCulture));

            if (ReceiveWindow.HasValue)
                result.Append("&recvWindow=").Append(ReceiveWindow.Value);

            return result.ToString();
        }
    }

    public enum OrderSide
    {
        Buy, Sell
    }

    public enum OrderType
    {
        Limit, Stop
    }
}