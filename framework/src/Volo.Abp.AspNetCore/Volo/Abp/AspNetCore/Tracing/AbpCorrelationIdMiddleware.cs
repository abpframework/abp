using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Volo.Abp.Tracing;

namespace Volo.Abp.AspNetCore.Tracing
{
    public class AbpCorrelationIdMiddleware
    {
        private readonly RequestDelegate _next;

        public AbpCorrelationIdMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(
            HttpContext httpContext,
            IOptions<CorrelationIdOptions> options,
            ICorrelationIdProvider correlationIdProvider)
        {
            var correlationId = correlationIdProvider.Get();
            var optionsValue = options.Value;

            try
            {
                await _next(httpContext);
            }
            finally
            {
                CheckAndSetCorrelationIdOnResponse(httpContext, optionsValue, correlationId);
            }
        }

        protected virtual void CheckAndSetCorrelationIdOnResponse(
            HttpContext httpContext,
            CorrelationIdOptions options,
            string correlationId)
        {
            if (httpContext.Response.HasStarted)
            {
                return;
            }

            if (!options.SetResponseHeader)
            {
                return;
            }

            if (httpContext.Response.Headers.ContainsKey(options.HttpHeaderName))
            {
                return;
            }

            httpContext.Response.Headers[options.HttpHeaderName] = correlationId;
        }
    }
}
