using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Authorization;
using Volo.Abp.Dapr;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Mvc.Dapr;

public class DaprAppApiTokenValidator : IDaprAppApiTokenValidator, ISingletonDependency
{
    public virtual void CheckDaprAppApiToken(HttpContext httpContext)
    {
        var expectedAppApiToken = GetConfiguredAppApiToken(httpContext);
        if (expectedAppApiToken.IsNullOrWhiteSpace())
        {
            return;
        }
        
        var headerAppApiToken = GetDaprAppApiTokenOrNull(httpContext);
        if (headerAppApiToken.IsNullOrWhiteSpace())
        {
            throw new AbpAuthorizationException("Expected Dapr App API Token is not provided! Dapr should set the 'dapr-api-token' HTTP header.");
        }

        if (expectedAppApiToken != headerAppApiToken)
        {
            throw new AbpAuthorizationException("The Dapr App API Token (provided in the 'dapr-api-token' HTTP header) doesn't match the expected value!");
        }
    }

    public virtual bool IsValidDaprAppApiToken(HttpContext httpContext)
    {
        var expectedAppApiToken = GetConfiguredAppApiToken(httpContext);
        if (expectedAppApiToken.IsNullOrWhiteSpace())
        {
            return true;
        }
        
        var headerAppApiToken = GetDaprAppApiTokenOrNull(httpContext);
        return expectedAppApiToken == headerAppApiToken;
    }

    public virtual string? GetDaprAppApiTokenOrNull(HttpContext httpContext)
    {
        string apiTokenHeader = httpContext.Request.Headers["dapr-api-token"];
        if (string.IsNullOrEmpty(apiTokenHeader) || apiTokenHeader.Length < 1)
        {
            return null;
        }

        return apiTokenHeader;
    }
    
    protected virtual string GetConfiguredAppApiToken(HttpContext httpContext)
    {
        var expectedAppApiToken = httpContext
            .RequestServices
            .GetRequiredService<IDaprApiTokenProvider>()
            .GetAppApiToken();
        return expectedAppApiToken;
    }
}