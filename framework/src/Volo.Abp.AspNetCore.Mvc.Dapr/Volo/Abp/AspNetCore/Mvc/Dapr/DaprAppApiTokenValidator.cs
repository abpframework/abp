using System;
using System.Threading.Tasks;
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

    public virtual async Task CheckDaprAppApiTokenAsync()
    {
        var expectedAppApiToken = await GetConfiguredAppApiTokenOrNullAsync();
        if (expectedAppApiToken.IsNullOrWhiteSpace())
        {
            return;
        }

        var headerAppApiToken = await GetDaprAppApiTokenOrNullAsync();
        if (headerAppApiToken.IsNullOrWhiteSpace())
        {
            throw new AbpAuthorizationException("Expected Dapr App API Token is not provided! Dapr should set the 'dapr-api-token' HTTP header.");
        }

        if (expectedAppApiToken != headerAppApiToken)
        {
            throw new AbpAuthorizationException("The Dapr App API Token (provided in the 'dapr-api-token' HTTP header) doesn't match the expected value!");
        }
    }

    public virtual async Task<bool> IsValidDaprAppApiTokenAsync()
    {
        var expectedAppApiToken = await GetConfiguredAppApiTokenOrNullAsync();
        if (expectedAppApiToken.IsNullOrWhiteSpace())
        {
            return true;
        }

        var headerAppApiToken = await GetDaprAppApiTokenOrNullAsync();
        return expectedAppApiToken == headerAppApiToken;
    }

    public virtual Task<string> GetDaprAppApiTokenOrNullAsync()
    {
        string apiTokenHeader = HttpContext.Request.Headers["dapr-api-token"];
        if (string.IsNullOrEmpty(apiTokenHeader) || apiTokenHeader.Length < 1)
        {
            return Task.FromResult<string>(null);
        }

        return Task.FromResult(apiTokenHeader);
    }

    protected virtual async Task<string> GetConfiguredAppApiTokenOrNullAsync()
    {
        return await HttpContext
            .RequestServices
            .GetRequiredService<IDaprApiTokenProvider>()
            .GetAppApiTokenAsync();
    }

    protected virtual HttpContext GetHttpContext()
    {
        return HttpContextAccessor.HttpContext ?? throw new AbpException("HttpContext is not available!");
    }
}
