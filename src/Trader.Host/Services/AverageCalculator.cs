using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Trader.Host.Services
{
    public class AverageCalculator
    {
        private readonly int _maxSamples;
        private ConcurrentQueue<decimal> _queue = new ConcurrentQueue<decimal>();

        public AverageCalculator(int maxSamples)
        {
            _maxSamples = maxSamples;
        }

        public void AddSample(decimal value)
        {
            _queue.Enqueue(value);

            if (_queue.Count <= _maxSamples)
                return;

            while (!_queue.TryDequeue(out var tmp))
                Thread.Sleep(5);
        }

        public int Count => _queue.Count;

        public IEnumerable<decimal> Samples => _queue.ToList();

        public decimal Calculate()
        {
            return Count == 0 ? 0 : _queue.Average();
        }

        public decimal Calculate(decimal currentValue)
        {
            return (_queue.Sum() + currentValue) / (Count + 1);
        }

        public void Reset()
        {
            _queue = new ConcurrentQueue<decimal>();
        }
    }
}