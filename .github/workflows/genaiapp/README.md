# GenAI Deployment Platform Backend

This is a .NET 9 Web API backend for a GenAI-powered deployment platform. It provides:

- Intelligent Deployment Orchestration (bulk, AI-driven)
- Predictive Quality Gates (AI code review, test generation)
- Proactive Monitoring & Alerting
- Cost Optimization
- Security & Compliance (SSO, RBAC, audit logging)

## Getting Started

1. Ensure you have .NET 9 SDK installed.
2. Restore dependencies:
   ```powershell
   dotnet restore
   ```
3. Build the project:
   ```powershell
   dotnet build
   ```
4. Run the API:
   ```powershell
   dotnet run
   ```

## Project Structure
- `Controllers/` — API endpoints
- `Program.cs` — App entry point
- `appsettings.json` — Configuration

## Security & Compliance
- SSO, RBAC, and audit logging are required for all endpoints.
- All AI decisions are logged and require human approval for high-risk changes.
- Sensitive data is never sent to external APIs unless explicitly allowed.

---

For more details, see `.github/copilot-instructions.md`.
