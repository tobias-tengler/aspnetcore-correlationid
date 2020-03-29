using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

public class CorrelationMiddleware
{
    private readonly RequestDelegate _next;

    public CorrelationMiddleware(RequestDelegate next)
        => _next = next;

    public Task InvokeAsync(HttpContext context)
    {
        var logger = context.RequestServices.GetRequiredService<ILogger<CorrelationMiddleware>>();

        var correlationIdHeader = context.Request.Headers["X-CorrelationId"].FirstOrDefault();

        // parsing the header to a guid ensures it is not malicious data
        if (string.IsNullOrEmpty(correlationIdHeader) || !Guid.TryParse(correlationIdHeader, out var correlationId))
        {
            // this should not go in production as it will introduce an unnecessary performance hit
            logger.LogDebug("CorrelationId header was not set");

            correlationId = Guid.NewGuid();
        }

        // add it to the context so we can access it within our controllers, etc.
        context.Items["CorrelationId"] = correlationId;

        // add it as a variable to our logger, so it shows up in all logs that happen during this request
        using (logger.BeginScope("{@CorrelationId}", correlationId))
        {
            return _next(context);
        }
    }
}