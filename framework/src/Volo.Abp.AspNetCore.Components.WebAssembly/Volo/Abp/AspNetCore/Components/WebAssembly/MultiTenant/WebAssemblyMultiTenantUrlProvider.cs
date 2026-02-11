using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Text.Formatting;

namespace Volo.Abp.AspNetCore.Components.WebAssembly.MultiTenant;

[Dependency(ReplaceServices = true)]
[ExposeServices(typeof(IMultiTenantUrlProvider), typeof(MultiTenantUrlProvider))]
public class WebAssemblyMultiTenantUrlProvider : MultiTenantUrlProvider
{
    private readonly static string[] ProtocolPrefixes = ["http://", "https://"];

    protected NavigationManager NavigationManager { get; }
    protected IOptions<WebAssemblyMultiTenantUrlOptions> Options { get; }

    public WebAssemblyMultiTenantUrlProvider(
        ICurrentTenant currentTenant,
        ITenantStore tenantStore,
        NavigationManager navigationManager,
        IOptions<WebAssemblyMultiTenantUrlOptions> options)
        : base(currentTenant, tenantStore)
    {
        NavigationManager = navigationManager;
        Options = options;
    }

    public async override Task<string> GetUrlAsync(string templateUrl)
    {
        if (!Options.Value.DomainFormat.IsNullOrEmpty() && !CurrentTenant.IsAvailable)
        {
            // If the domain format is configured and the tenant is not available
            // try to extract the tenant name from the current blazor URL.
            var url = NavigationManager.ToAbsoluteUri(NavigationManager.Uri).Authority;
            var domainFormat = Options.Value.DomainFormat.RemovePreFix(ProtocolPrefixes).RemovePostFix("/");
            var extractResult = FormattedStringValueExtracter.Extract(url, domainFormat, ignoreCase: true);
            if (extractResult.IsMatch)
            {
                var tenant = extractResult.Matches[0].Value;
                return templateUrl.Replace(TenantPlaceHolder, tenant).Replace(TenantIdPlaceHolder, tenant).Replace(TenantNamePlaceHolder, tenant);
            }
        }

        return await base.GetUrlAsync(templateUrl);
    }
}
