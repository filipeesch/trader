using System;
using System.Collections.Concurrent;
using System.Configuration;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Trader.Host.Infrastruture;
using Trader.Host.Messages;
using Trader.Host.WebSocket.Listeners;

namespace Trader.Host.WebSocket.Core
{
    public class BinanceUserWebSocket : IDisposable
    {
        private const string AccountInfoEventName = "outboundAccountInfo";
        private const string ExecutionReportEventName = "executionReport";

        private readonly WebSocketListener _webSocket;

        private Action<OnAccountInfoMessage> _onAccountInfoUpdate;
        private Action<OnExecutionReportMessage> _onExecutionReport;

        private readonly ILogger _logger;

        public BinanceUserWebSocket()
        {
            _webSocket = new WebSocketListener();
            _logger = new ConsoleLogger();
        }

        private void OnMessageArrives(string rawMessage)
        {
            try
            {
                var message = JObject.Parse(rawMessage);

                var eventName = message["e"].Value<string>();

                switch (eventName)
                {
                    case AccountInfoEventName:
                        AccountInfoUpdate(message.ToObject<OnAccountInfoMessage>());
                        break;

                    case ExecutionReportEventName:
                        ExecutionReport(message.ToObject<OnExecutionReportMessage>());
                        break;
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Error on translating user stream message", ex);
            }
        }

        private void AccountInfoUpdate(OnAccountInfoMessage message)
        {
            _onAccountInfoUpdate?.Invoke(message);
        }

        private void ExecutionReport(OnExecutionReportMessage message)
        {
            _onExecutionReport?.Invoke(message);
        }

        public async Task Start(string listenKey)
        {
            var uri = new StringBuilder(128);

            var baseAddress = ConfigurationManager.AppSettings["WebSocketsBaseAddress"];

            uri.Append(baseAddress).Append("/ws/").Append(listenKey);

            await _webSocket.Connect(uri.ToString());

            _webSocket.OnMessage(OnMessageArrives);
        }

        public void Dispose()
        {
            _webSocket.Dispose();
        }

        public void OnAccountInfoUpdate(Action<OnAccountInfoMessage> action)
        {
            _onAccountInfoUpdate = action;
        }

        public void OnExecutionReportUpdate(Action<OnExecutionReportMessage> action)
        {
            _onExecutionReport = action;
        }
    }
}