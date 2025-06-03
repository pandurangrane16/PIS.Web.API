using System.Net;
using System.Text.Json;
using PIS.User.Core.Services;

namespace PIS.User.API.Extensions
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                // Resolve scoped service properly
                var errorLogger = context.RequestServices.GetRequiredService<IErrorLog>();
                //errorLogger.LogError(ex);

                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json";

                var result = JsonSerializer.Serialize(new { error = ex.Message });
                await context.Response.WriteAsync(result);
            }
        }
    }
}
