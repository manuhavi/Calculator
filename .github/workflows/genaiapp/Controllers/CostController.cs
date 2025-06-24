using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace GenAIApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CostController : ControllerBase
    {
        // GET: api/cost/optimization
        [HttpGet("optimization")]
        [Authorize]
        public IActionResult GetOptimization()
        {
            // TODO: Implement AI-driven cost optimization and usage prediction
            return Ok(new { status = "Optimization calculated", savings = "35%" });
        }
    }
}
