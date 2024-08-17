using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Authorization;
using Volo.Abp.Dapr;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Mvc.Dapr;

public class DaprAppApiTokenValidator : IDaprAppApiTokenValidator, ISingletonDependency
{
    protected IHttpContextAccessor HttpContextAccessor { get; }
    protected HttpContext HttpContext => GetHttpContext();

    public DaprAppApiTokenValidator(IHttpContextAccessor httpContextAccessor)
    {
        HttpContextAccessor = httpContextAccessor;
    }

    public virtual void CheckDaprAppApiToken()
    {
        var expectedAppApiToken = GetConfiguredAppApiTokenOrNull();
        if (expectedAppApiToken.IsNullOrWhiteSpace())
        {
            return;
        }

        var headerAppApiToken = GetDaprAppApiTokenOrNull();
        if (headerAppApiToken.IsNullOrWhiteSpace())
        {
            throw new AbpAuthorizationException("Expected Dapr App API Token is not provided! Dapr should set the 'dapr-api-token' HTTP header.");
        }

        if (expectedAppApiToken != headerAppApiToken)
        {
            throw new AbpAuthorizationException("The Dapr App API Token (provided in the 'dapr-api-token' HTTP header) doesn't match the expected value!");
        }
    }

    public virtual bool IsValidDaprAppApiToken()
    {
        var expectedAppApiToken = GetConfiguredAppApiTokenOrNull();
        if (expectedAppApiToken.IsNullOrWhiteSpace())
        {
            return true;
        }

        var headerAppApiToken = GetDaprAppApiTokenOrNull();
        return expectedAppApiToken == headerAppApiToken;
    }

    public virtual string? GetDaprAppApiTokenOrNull()
    {
        string? apiTokenHeader = HttpContext.Request.Headers["dapr-api-token"];
        if (string.IsNullOrEmpty(apiTokenHeader) || apiTokenHeader.Length < 1)
        {
            return null;
        }

        return apiTokenHeader;
    }

    protected virtual string? GetConfiguredAppApiTokenOrNull()
    {
        return HttpContext
            .RequestServices
            .GetRequiredService<IDaprApiTokenProvider>()
            .GetAppApiToken();
    }

    protected virtual HttpContext GetHttpContext()
    {
        return HttpContextAccessor.HttpContext ?? throw new AbpException("HttpContext is not available!");
    }
}
