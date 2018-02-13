using System;

namespace Trader.Host.Infrastruture
{
    public class ConsoleLogger : ILogger
    {
        public void Error(string message, Exception ex = null)
        {
            if (ex != null)
                Console.Error.WriteLine(message + ": " + ex.Message);
            else
                Console.Error.WriteLine(message);
        }
    }
}
