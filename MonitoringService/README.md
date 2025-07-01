# MonitoringService

A simple .NET 9 background service for proactive monitoring and auto-remediation using GenAI (OpenAI API).

## Features
- Monitors application logs for anomalies or failure patterns
- Uses OpenAI API to predict incidents from logs
- Triggers auto-remediation (example: creates a file, can be extended)

## Usage
1. Set your OpenAI API key as the environment variable `OPENAI_API_KEY`.
2. Place your application logs at `../logs/app.log` (or adjust the path in `Program.cs`).
3. Run the service:
   ```sh
   dotnet run --project MonitoringService
   ```

## Extending
- Replace the remediation action in `LogMonitor.cs` with real scripts (restart service, send alert, etc).
- Integrate with incident management tools (GitHub Issues, ServiceNow, etc).

---
This is a minimal GenAI-powered monitoring and remediation example for .NET projects.
