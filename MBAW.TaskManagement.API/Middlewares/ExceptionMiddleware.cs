using System.Net;
using System.Text.Json;

namespace MBAW.TaskManagement.API.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            // Log the exception if needed (e.g., using ILogger)
            // logger.LogError(exception, "An unhandled exception occurred.");

            var response = new
            {
                StatusCode = (int)HttpStatusCode.InternalServerError,
                Data = default(object),
                Messages = new List<string> { exception.Message }
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.OK; // Always return HTTP 200 OK

            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
