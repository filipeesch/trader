using System;

namespace Trader.Host.Infrastruture
{
    public interface ILogger
    {
        void Error(string message, Exception ex);
    }
}