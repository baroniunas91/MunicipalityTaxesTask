using MunicipalityTaxesAPI.Exceptions;
using MunicipalityTaxesAPI.Models;

namespace MunicipalityTaxesAPI.Middlewares
{
    public class HttpStatusExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<HttpStatusExceptionMiddleware> _logger;

        public HttpStatusExceptionMiddleware(RequestDelegate next, ILogger<HttpStatusExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (HttpStatusException e) when ((int)e.Status < 500)
            {
                context.Response.StatusCode = (int)e.Status;
                await context.Response.WriteAsJsonAsync(new ResponseMessage(e.Message));
            }
            catch (Exception e)
            {
                // Internal server errors handling
                context.Response.StatusCode = 400;
                _logger.LogError(e, "{ExceptionObject}", e.ToString());
                await context.Response.WriteAsJsonAsync(new ResponseMessage("Request Aborted"));
            }
        }
    }
}
