using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Identity.AspNetCore;

[Dependency(ReplaceServices = true)]
public class HttpContextIdentitySessionValidationResultAccessor : IIdentitySessionValidationResultAccessor, ITransientDependency
{
    public const string HttpContextItemName = "__AbpIdentitySessionValidationResults";

    private readonly IHttpContextAccessor _httpContextAccessor;

    public HttpContextIdentitySessionValidationResultAccessor(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public bool? GetOrNull(string sessionId)
    {
        return GetResults()?.TryGetValue(sessionId, out var isValid) == true ? isValid : null;
    }

    public void Set(string sessionId, bool isValid)
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext == null)
        {
            return;
        }

        var results = GetResults();
        if (results == null)
        {
            results = new Dictionary<string, bool>();
            httpContext.Items[HttpContextItemName] = results;
        }

        results[sessionId] = isValid;
    }

    private Dictionary<string, bool> GetResults()
    {
        return _httpContextAccessor.HttpContext?.Items[HttpContextItemName] as Dictionary<string, bool>;
    }
}
