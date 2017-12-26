using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trader.Host.Helpers
{
    public class DateHelpers
    {
        public static DateTime FromBinanceDate(long value)
        {
            return new DateTime(1970, 1, 1) + TimeSpan.FromMilliseconds(value);
        }

        public static long ToBinanceDate(DateTime value)
        {
            return Convert.ToInt64((value - new DateTime(1970, 1, 1)).TotalMilliseconds);
        }
    }
}
