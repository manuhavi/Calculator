using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace GenAIApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MonitoringController : ControllerBase
    {
        // GET: api/monitoring/health
        [HttpGet("health")]
        [Authorize]
        public IActionResult Health()
        {
            // Simulate AI-driven health and failure prediction
            var prediction = "No failures predicted";
            var status = "Healthy";
            var aiDecision = new {
                Timestamp = DateTime.UtcNow,
                User = User?.Identity?.Name ?? "anonymous",
                Endpoint = "GET /api/monitoring/health",
                Prediction = prediction,
                Status = status
            };
            // Log AI decision for audit (using IAuditLogger)
            var logger = HttpContext.RequestServices.GetService(typeof(IAuditLogger)) as IAuditLogger;
            logger?.LogRequestAsync(HttpContext);
            // Optionally, log AI decision details
            // logger?.LogAIDecisionAsync(aiDecision); // Extend IAuditLogger for full traceability
            return Ok(new { status, prediction, audit = aiDecision });
        }
    }
}
