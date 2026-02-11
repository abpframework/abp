using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.UI.Navigation.Urls;

public class AppUrlProvider : IAppUrlProvider, ITransientDependency
{
    protected AppUrlOptions Options { get; }
    protected IMultiTenantUrlProvider MultiTenantUrlProvider { get; }
    public ILogger<AppUrlProvider> Logger { get; set; }

    public AppUrlProvider(
        IOptions<AppUrlOptions> options,
        IMultiTenantUrlProvider multiTenantUrlProvider)
    {
        MultiTenantUrlProvider = multiTenantUrlProvider;
        Options = options.Value;
        Logger = NullLogger<AppUrlProvider>.Instance;
    }

    public virtual async Task<string> GetUrlAsync(string appName, string? urlName = null)
    {
        return await MultiTenantUrlProvider.GetUrlAsync(
            await GetConfiguredUrl(
                appName,
                urlName
            )
        );
    }

    public virtual async Task<bool> IsRedirectAllowedUrlAsync(string url)
    {
        var redirectAllowedUrls = new List<string>();
        foreach (var redirectAllowedUrl in Options.RedirectAllowedUrls)
        {
            redirectAllowedUrls.Add((await NormalizeUrlAsync(redirectAllowedUrl))!);
        }
        var allow = redirectAllowedUrls.Any(x => url.StartsWith(x, StringComparison.CurrentCultureIgnoreCase) ||
                                                 UrlHelpers.IsSubdomainOf(url, x));
        if (!allow)
        {
            Logger.LogError($"Invalid RedirectUrl: {url}, Use {nameof(AppUrlProvider)} to configure it!");
        }
        return allow;
    }

    public virtual async Task<string?> NormalizeUrlAsync(string? url)
    {
        if (string.IsNullOrWhiteSpace(url))
        {
            return url;
        }

        return await MultiTenantUrlProvider.GetUrlAsync(url!);
    }

    protected virtual async Task<string> GetConfiguredUrl(string appName, string? urlName)
    {
        var url = await GetUrlOrNullAsync(appName, urlName);
        if (!url.IsNullOrEmpty())
        {
            return url!;
        }

        if (!urlName.IsNullOrEmpty())
        {
            throw new AbpException(
                $"Url, named '{urlName}', for the application '{appName}' was not configured. Use {nameof(AppUrlOptions)} to configure it!"
            );
        }

        throw new AbpException(
            $"RootUrl for the application '{appName}' was not configured. Use {nameof(AppUrlOptions)} to configure it!"
        );
    }

    public Task<string?> GetUrlOrNullAsync([NotNull] string appName, string? urlName = null)
    {
        var app = Options.Applications[appName];

        if (urlName.IsNullOrEmpty())
        {
            return Task.FromResult(app.RootUrl);
        }

        var url = app.Urls.GetOrDefault(urlName!);

        if (app.RootUrl == null)
        {
            return Task.FromResult(url);
        }

        return Task.FromResult<string?>(app.RootUrl.EnsureEndsWith('/') + url);
    }
}
