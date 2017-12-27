using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trader.Host.Helpers
{
    public static class ColletionExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> collection, Action<T> handler)
        {
            foreach (var item in collection)
                handler(item);
        }
    }
}
