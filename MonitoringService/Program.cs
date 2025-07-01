using System;
using System.Threading;
using System.Threading.Tasks;

namespace MonitoringService
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string logFilePath = "logs/app.log";
            var monitor = new LogMonitor(logFilePath);
            var cts = new CancellationTokenSource();
            Console.CancelKeyPress += (s, e) =>
            {
                e.Cancel = true;
                cts.Cancel();
            };
            Console.WriteLine("Monitoring started. Press Ctrl+C to exit.");
            await monitor.MonitorAsync(cts.Token);
        }
    }
}
