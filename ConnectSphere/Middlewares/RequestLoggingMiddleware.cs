using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace ConnectSphere.WebApi.Middleware;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(
        RequestDelegate next,
        ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();

        // Scope already has Method, Path, CorrelationId
        _logger.LogInformation("Request started");

        try
        {
            await _next(context);
            stopwatch.Stop();

            //Only log what's NEW
            _logger.LogInformation(
                "Request completed {StatusCode} in {ElapsedMs} ms",
                context.Response.StatusCode,
                stopwatch.ElapsedMilliseconds);
        }
        catch (Exception ex)
        {
            stopwatch.Stop();

            _logger.LogError(
                ex,
                "Request failed in {ElapsedMs} ms",
                stopwatch.ElapsedMilliseconds);

            throw;
        }
    }
}