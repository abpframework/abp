using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http;
using Volo.Abp.Uow;

namespace Volo.Abp.AspNetCore.Uow;

public class AspNetCoreUnitOfWorkTransactionBehaviourProvider : IUnitOfWorkTransactionBehaviourProvider, ISingletonDependency
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly AspNetCoreUnitOfWorkTransactionBehaviourProviderOptions _options;

    public virtual bool? IsTransactional {
        get {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null)
            {
                return null;
            }

            if (httpContext.WebSockets.IsWebSocketRequest)
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

            var method = httpContext.Request.Method;
            return !(HttpMethodHelper.IsGet(method) || HttpMethodHelper.IsQuery(method));
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
