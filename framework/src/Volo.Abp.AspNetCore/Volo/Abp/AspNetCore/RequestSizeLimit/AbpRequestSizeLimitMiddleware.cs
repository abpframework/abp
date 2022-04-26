using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.RequestSizeLimit;

public class AbpRequestSizeLimitMiddleware : IMiddleware, ITransientDependency
{
    private readonly ILogger<AbpRequestSizeLimitMiddleware> _logger;

    public AbpRequestSizeLimitMiddleware(ILogger<AbpRequestSizeLimitMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var endpoint = context.GetEndpoint();
        if (endpoint != null)
        {
            if (!endpoint.Metadata.Any(x => x is RequestSizeLimitAttribute || x is DisableRequestSizeLimitAttribute))
            {
                var attribute = endpoint.Metadata.GetMetadata<AbpRequestSizeLimitAttribute>();
                if (attribute != null)
                {
                    var maxRequestBodySizeFeature = context.Features.Get<IHttpMaxRequestBodySizeFeature>();
                    if (maxRequestBodySizeFeature == null)
                    {
                        _logger.LogInformation("A request body size limit could not be applied. This server does not support the IHttpRequestBodySizeFeature.");
                    }
                    else if (maxRequestBodySizeFeature.IsReadOnly)
                    {
                        _logger.LogInformation("A request body size limit could not be applied. The IHttpRequestBodySizeFeature for the server is read-only.");
                    }
                    else
                    {
                        _logger.LogInformation($"The maximum request body size has been set to {attribute.GetBytes().ToString(CultureInfo.InvariantCulture)}.");
                        maxRequestBodySizeFeature.MaxRequestBodySize = attribute.GetBytes();
                    }
                }
                else
                {
                    _logger.LogInformation($"AbpRequestSizeLimitAttribute does not exist in endpoint, Skipping.");
                }
            }
            else
            {
                _logger.LogInformation($"Endpoint already exists IRequestSizePolicy, Skipping.");
            }
        }
        else
        {
            _logger.LogInformation($"Endpoint is null, Skipping.");
        }

        await next(context);
    }
}
