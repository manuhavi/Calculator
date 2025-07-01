using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace MonitoringService
{
    public class LogMonitor
    {
        private readonly string _logFilePath;
        private readonly string _geminiApiKey;
        private readonly string _geminiEndpoint;
        private readonly HttpClient _httpClient;

        public LogMonitor(string logFilePath)
        {
            _logFilePath = logFilePath;
            _geminiApiKey = "AIzaSyDR9MkXL4aVgffFBU0bdpN1iPy6293s-BI";
            _geminiEndpoint = "https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent?key=" + _geminiApiKey;
            _httpClient = new HttpClient();
        }

        public async Task MonitorAsync(CancellationToken cancellationToken)
        {
            int maxCycles = 5; // Limit the number of monitoring cycles
            int cycle = 0;
            while (!cancellationToken.IsCancellationRequested && cycle < maxCycles)
            {
                Console.WriteLine($"[{DateTime.UtcNow}] Reading logs from {_logFilePath}...");
                string logs = await File.ReadAllTextAsync(_logFilePath, cancellationToken);
                Console.WriteLine($"[{DateTime.UtcNow}] Sending logs to Gemini API for prediction...");
                bool isFailurePredicted = await PredictFailureAsync(logs);
                Console.WriteLine($"[{DateTime.UtcNow}] Gemini API prediction: {(isFailurePredicted ? "FAILURE" : "OK")}");
                if (isFailurePredicted)
                {
                    Console.WriteLine($"[{DateTime.UtcNow}] Triggering remediation...");
                    await TriggerRemediationAsync();
                }
                Console.WriteLine($"[{DateTime.UtcNow}] Waiting for next cycle...\n");
                await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
                cycle++;
            }
            Console.WriteLine($"[{DateTime.UtcNow}] Monitoring finished after {cycle} cycles.");
        }

        private async Task<bool> PredictFailureAsync(string logs)
        {
            var requestBody = new
            {
                contents = new[]
                {
                    new {
                        parts = new[]
                        {
                            new { text = $"Analyze the following logs and predict if a failure or incident is likely. Respond only with 'true' or 'false'. Logs: {logs}" }
                        }
                    }
                }
            };
            var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
            _httpClient.DefaultRequestHeaders.Clear();
            var start = DateTime.UtcNow;
            var response = await _httpClient.PostAsync(_geminiEndpoint, content);
            var duration = DateTime.UtcNow - start;
            string result = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"[{DateTime.UtcNow}] Gemini API response time: {duration.TotalSeconds:F2}s");
            Console.WriteLine($"[{DateTime.UtcNow}] Gemini API raw response: {result}");
            return result.Contains("true");
        }

        private async Task TriggerRemediationAsync()
        {
            // Example: create a file as a remediation action (replace with real script or alert)
            await File.WriteAllTextAsync("remediation_triggered.txt", $"Remediation triggered at {DateTime.UtcNow}");
        }
    }
}
