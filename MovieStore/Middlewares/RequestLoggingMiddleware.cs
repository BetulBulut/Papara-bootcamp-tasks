using Microsoft.IO;
using Serilog;

namespace MovieStore.Middleware;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate next;
    private readonly RecyclableMemoryStreamManager recyclableMemoryStreamManager;
    private readonly Action<RequestProfilerModel> requestResponseHandler;
    private const int ReadChunkBufferLength = 4096;

    public RequestLoggingMiddleware(RequestDelegate next, Action<RequestProfilerModel> requestResponseHandler)
    {
        this.next = next;
        this.requestResponseHandler = requestResponseHandler;
        this.recyclableMemoryStreamManager = new RecyclableMemoryStreamManager();
    }

    public async Task Invoke(HttpContext context)
{
    Log.Information("RequestLoggingMiddleware started processing a request.");

    var model = new RequestProfilerModel
    {
        RequestTime = DateTimeOffset.Now,
        Context = context,
        Request = await FormatRequest(context)
    };

    Stream originalBody = context.Response.Body;

    // Use a recyclable memory stream for response body
    using (MemoryStream newResponseBody = recyclableMemoryStreamManager.GetStream())
    {
        context.Response.Body = newResponseBody;

        // Proceed with the next middleware in the pipeline
        await next(context);

        // Ensure that the response body is correctly copied to the original stream
        newResponseBody.Seek(0, SeekOrigin.Begin);
        await newResponseBody.CopyToAsync(originalBody);

        // Process the response after copying
        newResponseBody.Seek(0, SeekOrigin.Begin);
        model.Response = FormatResponse(context, newResponseBody);
        model.ResponseTime = DateTimeOffset.Now;

        // Log the request and response using Serilog
        Log.Information("Request: {Request}", model.Request);
        Log.Information("Response: {Response}", model.Response);

        requestResponseHandler(model);
    }
}


    private string FormatResponse(HttpContext context, MemoryStream newResponseBody)
    {
        HttpRequest request = context.Request;
        HttpResponse response = context.Response;

        return $"Http Response Information: {Environment.NewLine}" +
                $"Schema: {request.Scheme} {Environment.NewLine}" +
                $"Host: {request.Host} {Environment.NewLine}" +
                $"Path: {request.Path} {Environment.NewLine}" +
                $"QueryString: {request.QueryString} {Environment.NewLine}" +
                $"StatusCode: {response.StatusCode} {Environment.NewLine}" +
                $"Response Body: {ReadStreamInChunks(newResponseBody)}";
    }

    private async Task<string> FormatRequest(HttpContext context)
    {
        HttpRequest request = context.Request;

        return $"Http Request Information: {Environment.NewLine}" +
                    $"Schema: {request.Scheme} {Environment.NewLine}" +
                    $"Host: {request.Host} {Environment.NewLine}" +
                    $"Path: {request.Path} {Environment.NewLine}" +
                    $"QueryString: {request.QueryString} {Environment.NewLine}" +
                    $"Request Body: {await GetRequestBody(request)}";
    }

    public async Task<string> GetRequestBody(HttpRequest request)
    {
        request.EnableBuffering();
        using (var requestStream = recyclableMemoryStreamManager.GetStream())
        {
            await request.Body.CopyToAsync(requestStream);
            request.Body.Seek(0, SeekOrigin.Begin);
            return ReadStreamInChunks(requestStream);
        }
    }

    private static string ReadStreamInChunks(Stream stream)
    {
        stream.Seek(0, SeekOrigin.Begin);
        string result;
        using (var textWriter = new StringWriter())
        using (var reader = new StreamReader(stream))
        {
            var readChunk = new char[ReadChunkBufferLength];
            int readChunkLength;

            do
            {
                readChunkLength = reader.ReadBlock(readChunk, 0, ReadChunkBufferLength);
                textWriter.Write(readChunk, 0, readChunkLength);
            } while (readChunkLength > 0);

            result = textWriter.ToString();
        }

        return result;
    }
}

public class RequestProfilerModel
{
    public DateTimeOffset RequestTime { get; set; }
    public HttpContext Context { get; set; }
    public string Request { get; set; }
    public string Response { get; set; }
    public DateTimeOffset ResponseTime { get; set; }
}