using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

public class TestController : ControllerBase
{
    private readonly ILogger<TestController> _logger;
    private readonly ICorrelationIdAccessor _correlationIdAccessor;

    public TestController(ILogger<TestController> logger, ICorrelationIdAccessor correlationIdAccessor)
        => (_logger, _correlationIdAccessor) = (logger, correlationIdAccessor);

    // sending a GET request to http://localhost:5000/test will invoke this method
    [HttpGet("test")]
    public void Test()
    {
        var correlationId = _correlationIdAccessor.GetCorrelationId();

        if (correlationId != null)
            _logger.LogInformation("The current CorrelationId is \"{CurrentCorrelationId}\"", correlationId);

        _logger.LogInformation("This is a test.");
    }
}