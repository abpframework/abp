using System;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Uow;

namespace Volo.Abp.AspNetCore.Uow;

public class AspNetCoreUnitOfWorkTransactionBehaviourProvider : IUnitOfWorkTransactionBehaviourProvider, ISingletonDependency
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly AspNetCoreUnitOfWorkTransactionBehaviourProviderOptions _options;

    public virtual bool? IsTransactional
    {
        get
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null)
            {
                return null;
            }

            var currentUrl = httpContext.Request.Path.Value;
            if (currentUrl != null)
            {
                foreach (var url in _options.NonTransactionalUrls)
                {
                    if (currentUrl.StartsWith(url, StringComparison.OrdinalIgnoreCase))
                    {
                        return false;
                    }
                }
            }

            return !string.Equals(
                httpContext.Request.Method,
                HttpMethod.Get.Method, StringComparison.OrdinalIgnoreCase
            );
        }
    }

    public AspNetCoreUnitOfWorkTransactionBehaviourProvider(
        IHttpContextAccessor httpContextAccessor,
        IOptions<AspNetCoreUnitOfWorkTransactionBehaviourProviderOptions> options)
    {
        _httpContextAccessor = httpContextAccessor;
        _options = options.Value;
    }
}
