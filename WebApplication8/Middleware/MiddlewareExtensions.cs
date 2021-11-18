using Microsoft.AspNetCore.Builder;

namespace WebApplication8.Middleware
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseConventionalMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ConventionalMiddleware>();
        }

        public static IApplicationBuilder UseFactoryActivatedMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<FactoryActivatedMiddleware>();
        }
    }
}
