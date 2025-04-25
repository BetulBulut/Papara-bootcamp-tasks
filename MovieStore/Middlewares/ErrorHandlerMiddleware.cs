using Serilog;

namespace MovieStore.Middleware;

public class ErrorHandlerMiddleware
{
    public readonly RequestDelegate next;

    public ErrorHandlerMiddleware(RequestDelegate next)
    {
        this.next = next;
    }

   public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            Log.Fatal($"Path: {context.Request.Path} {Environment.NewLine}" +
                    $"Method: {context.Request.Method} {Environment.NewLine}" +
                    $"QueryString: {context.Request.QueryString} {Environment.NewLine}" +
                    $"StatusCode: {context.Response.StatusCode} {Environment.NewLine}" +
                    $"Exception: {ex.Message}");

            Log.Fatal(ex, "An unhandled exception occurred while processing the request.");

            if (!context.Response.HasStarted)
            {
                context.Response.Clear(); // önceki yazılanları temizle
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = 500;

                var errorObj = new
                {
                    Message = "An unexpected error occurred.",
                    Detail = ex.Message
                };

                var errorJson = System.Text.Json.JsonSerializer.Serialize(errorObj);
                await context.Response.WriteAsync(errorJson);
            }
        }
    }

}