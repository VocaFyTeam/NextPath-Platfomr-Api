using VocaFy.NextPath.Platform.Shared.Infrastructure.Pipeline.Middleware.Components;

namespace VocaFy.NextPath.Platform.Shared.Infrastructure.Pipeline.Middleware.Extensions;
public static class MiddlewareExtensions
{
    [Obsolete("Use app.UseExceptionHandler() with AddExceptionHandler<GlobalExceptionHandler>() instead.")]
    public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<GlobalExceptionHandlerMiddleware>();
    }
}