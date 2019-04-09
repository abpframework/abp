using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Volo.Abp.AspNetCore.Uow;

namespace Volo.Docs.Utils
{
    public class GlobalExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<AbpUnitOfWorkMiddleware> _logger;

        public GlobalExceptionHandlerMiddleware(RequestDelegate next,  ILogger<AbpUnitOfWorkMiddleware> logger)
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

                httpContext.Response.Redirect("/");
            }
        }
    }
}