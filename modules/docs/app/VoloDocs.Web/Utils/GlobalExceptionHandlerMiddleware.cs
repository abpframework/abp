using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Volo.Abp.AspNetCore.Uow;
using Volo.Abp.Domain.Entities;
using Volo.Docs;

namespace VoloDocs.Web.Utils
{
    public class GlobalExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

        public GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger)
        {
            _logger = logger;
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError("Handled a global exception: " + ex.Message, ex);

                if (ex.Message.StartsWith("404 error") ||
                    ex is EntityNotFoundException ||
                    ex is DocumentNotFoundException)
                {
                    httpContext.Response.Redirect("/error/404");
                }
                else
                {
                    httpContext.Response.Redirect("/error/500");
                }
            }
        }
    }
}
