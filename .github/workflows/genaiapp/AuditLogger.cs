using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

public interface IAuditLogger
{
    Task LogRequestAsync(HttpContext context);
}

public class AuditLogger : IAuditLogger
{
    private readonly ILogger<AuditLogger> _logger;
    public AuditLogger(ILogger<AuditLogger> logger)
    {
        _logger = logger;
    }

    public Task LogRequestAsync(HttpContext context)
    {
        var user = context.User?.Identity?.Name ?? "anonymous";
        var path = context.Request.Path;
        var method = context.Request.Method;
        _logger.LogInformation("AUDIT: User {User} accessed {Method} {Path}", user, method, path);
        return Task.CompletedTask;
    }
}
