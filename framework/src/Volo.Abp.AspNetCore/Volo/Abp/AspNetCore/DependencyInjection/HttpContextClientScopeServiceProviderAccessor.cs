using System;
using Microsoft.AspNetCore.Http;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.DependencyInjection;

public class HttpContextClientScopeServiceProviderAccessor :
    IClientScopeServiceProviderAccessor,
    ISingletonDependency
{
    public IServiceProvider ServiceProvider
    {
        get
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null)
            {
                throw new AbpException("HttpContextClientScopeServiceProviderAccessor should only be used in a web request scope!");
            }

            return httpContext.RequestServices;
        }
    }

    private readonly IHttpContextAccessor _httpContextAccessor;

    public HttpContextClientScopeServiceProviderAccessor(
        IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
}
