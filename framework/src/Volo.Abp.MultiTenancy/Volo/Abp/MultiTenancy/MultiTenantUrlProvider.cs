using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.MultiTenancy;

public class MultiTenantUrlProvider : IMultiTenantUrlProvider , ITransientDependency
{
    public const string TenantPlaceHolder = "{0}";
    public const string TenantIdPlaceHolder = "{{tenantId}}";
    public const string TenantNamePlaceHolder = "{{tenantName}}";
    
    protected ICurrentTenant CurrentTenant { get; }
    protected ITenantStore TenantStore { get; }
    
    public MultiTenantUrlProvider(
        ICurrentTenant currentTenant,
        ITenantStore tenantStore)
    {
        CurrentTenant = currentTenant;
        TenantStore = tenantStore;
    }
    
    public virtual async Task<string> GetUrlAsync(string templateUrl)
    {
        return await ReplacePlaceHoldersAsync(templateUrl);
    }
    
    protected virtual async Task<string> ReplacePlaceHoldersAsync(string templateUrl)
    {
        templateUrl = await ReplacePlaceHolderAsync(templateUrl, TenantIdPlaceHolder);
        
        templateUrl = await ReplacePlaceHolderAsync(templateUrl, TenantNamePlaceHolder);

        templateUrl = await ReplacePlaceHolderAsync(templateUrl, TenantPlaceHolder);

        return templateUrl;
    }

    protected virtual async Task<string> ReplacePlaceHolderAsync(string templateUrl, string placeHolder)
    {
        if (!templateUrl.Contains(placeHolder))
        {
            return templateUrl;
        }
        
        var placeHolderValue = string.Empty;
        if (CurrentTenant.IsAvailable)
        {
            placeHolderValue = (placeHolder == TenantIdPlaceHolder ? CurrentTenant.Id!.Value.ToString() : await GetCurrentTenantNameAsync()) + ".";
        }
        
        if (templateUrl.Contains(placeHolder + '.'))
        {
            placeHolder += '.';
        }
        
        templateUrl = templateUrl.Replace(
            placeHolder,
            placeHolderValue
        );

        return templateUrl;
    }
    
    protected virtual async Task<string> GetCurrentTenantNameAsync()
    {
        if (CurrentTenant.Id.HasValue && CurrentTenant.Name.IsNullOrEmpty())
        {
            var tenantConfiguration = await TenantStore.FindAsync(CurrentTenant.Id.Value);
            return tenantConfiguration!.Name;
        }

        return CurrentTenant.Name!;
    }
}