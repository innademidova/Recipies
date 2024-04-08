using System.Diagnostics;
using ILogger = Serilog.ILogger;

namespace Recipes.Middlewares;

public class RequestDurationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;

    public RequestDurationMiddleware(RequestDelegate next, ILogger logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var sp = Stopwatch.StartNew();

        try
        {
            await _next(context);
        }
        finally
        {
            var elapsedMilliseconds = sp.ElapsedMilliseconds;

            if (elapsedMilliseconds > 300)
            {
                _logger.Warning("Request {Method} {Path} took {ElapsedMilliseconds}ms", context.Request.Method, context.Request.Path, elapsedMilliseconds);
            }
        }
    }
}