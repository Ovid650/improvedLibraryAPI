public class RequestResponseLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestResponseLoggingMiddleware> _logger;

    public RequestResponseLoggingMiddleware(RequestDelegate next, ILogger<RequestResponseLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Log Request
        _logger.LogInformation($"Incoming Request: {context.Request.Method} {context.Request.Path}");

        
        var originalBodyStream = context.Response.Body;

        using (var responseBody = new MemoryStream())
        {
            context.Response.Body = responseBody;

            await _next(context);

            // Log Response
            _logger.LogInformation($"Outgoing Response: {context.Response.StatusCode}");

            await responseBody.CopyToAsync(originalBodyStream);
        }
    }
}
