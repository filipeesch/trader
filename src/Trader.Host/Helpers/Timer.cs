using System;
using System.Threading.Tasks;

namespace Trader.Host.Helpers
{
    public class Timer
    {
        private bool _running = true;

        private Timer() { }

        private void Begin(int mseconds, Action action)
        {
            Task.Run(async () =>
            {
                while (_running)
                {
                    await Task.Delay(mseconds);

                    action();
                }
            });
        }

        public void Stop()
        {
            _running = false;
        }

        public static Timer RunEvery(int mseconds, Action action)
        {
            var t = new Timer();

            t.Begin(mseconds, action);

            return t;
        }
    }
}