using System;
using Microsoft.AspNetCore.Http;

public interface ICorrelationIdAccessor
{
    Guid? GetCorrelationId();
}

public class CorrelationIdAccessor : ICorrelationIdAccessor
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CorrelationIdAccessor(IHttpContextAccessor httpContextAccessor)
        => _httpContextAccessor = httpContextAccessor;

    public Guid? GetCorrelationId()
    {
        var context = _httpContextAccessor.HttpContext;

        if (context?.Items?["CorrelationId"] is Guid correlationId)
            return correlationId;

        return null;
    }
}