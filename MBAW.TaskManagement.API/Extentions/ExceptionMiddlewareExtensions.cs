using MBAW.TaskManagement.API.Middlewares;

namespace MBAW.TaskManagement.API.Extentions
{
    public static class ExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
