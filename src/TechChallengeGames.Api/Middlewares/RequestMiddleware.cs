namespace TechChallengeGames.Api.Middlewares;

public class RequestMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context, ILogger<RequestMiddleware> logger)
    {
        logger.LogInformation("Request: {RequestMethod} {RequestPath}", context.Request.Method, context.Request.Path);
        await next(context);
        logger.LogInformation("Response: {RequestMethod} {RequestPath} returned {ResponseStatusCode}", context.Request.Method, context.Request.Path, context.Response.StatusCode);
    }
}