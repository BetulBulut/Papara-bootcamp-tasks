namespace WebApi.Middlewares;

public class LoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<LoggingMiddleware> _logger;

    public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        var request = context.Request;
        var user = context.User.Identity;

        // Kullanıcı bilgisi (varsa)
        string userInfo = user?.IsAuthenticated == true
            ? $"User: {context.User.Identity.Name}"
            : "User: Anonymous";

        // Request bilgisi
        string logMessage = $"[{DateTime.UtcNow}] {request.Method} {request.Path} {request.QueryString} - {userInfo}";
        
        _logger.LogInformation(logMessage);

        await _next(context);
    }
}