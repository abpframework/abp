using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Mvc.AntiForgery;

public class AbpAntiForgeryCookieNameProvider : ITransientDependency
{
    private readonly IOptionsMonitor<CookieAuthenticationOptions> _namedOptionsAccessor;
    private readonly AbpAntiForgeryOptions _abpAntiForgeryOptions;

    public AbpAntiForgeryCookieNameProvider(
        IOptionsMonitor<CookieAuthenticationOptions> namedOptionsAccessor,
        IOptions<AbpAntiForgeryOptions> abpAntiForgeryOptions)
    {
        _namedOptionsAccessor = namedOptionsAccessor;
        _abpAntiForgeryOptions = abpAntiForgeryOptions.Value;
    }

    public virtual string GetAuthCookieNameOrNull()
    {
        if (_abpAntiForgeryOptions.AuthCookieSchemaName == null)
        {
            return null;
        }

        return _namedOptionsAccessor.Get(_abpAntiForgeryOptions.AuthCookieSchemaName)?.Cookie?.Name;
    }

    public virtual string GetAntiForgeryCookieNameOrNull()
    {
        return _abpAntiForgeryOptions.TokenCookie.Name;
    }
}
