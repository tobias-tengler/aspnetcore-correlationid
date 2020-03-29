using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

public class CorrelationMiddleware
{
    private readonly RequestDelegate _next;

    public CorrelationMiddleware(RequestDelegate next)
        => _next = next;

    public Task InvokeAsync(HttpContext context)
    {
        var correlationId = context.Request.Headers["CorrelationId"].FirstOrDefault();

        // If the header is present add it to the request context
        if (!string.IsNullOrEmpty(correlationId))
            context.Items["CorrelationId"] = correlationId;

        return _next(context);
    }
}