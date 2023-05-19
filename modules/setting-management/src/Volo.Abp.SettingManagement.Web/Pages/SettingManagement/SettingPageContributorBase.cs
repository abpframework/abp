using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.SettingManagement.Web.Pages.SettingManagement;

public abstract class SettingPageContributorBase : ISettingPageContributor
{
    private readonly HashSet<string> _requiredPermissions;
    private readonly HashSet<string> _requiredFeatures;
    private readonly HashSet<string> _requiredTenantSideFeatures;

    protected SettingPageContributorBase()
    {
        _requiredPermissions = new HashSet<string>();
        _requiredFeatures = new HashSet<string>();
        _requiredTenantSideFeatures = new HashSet<string>();
    }

    protected virtual SettingPageContributorBase RequiredPermissions(params string[] permissions)
    {
        Check.NotNullOrEmpty(permissions, nameof(permissions));
        _requiredPermissions.AddIfNotContains(permissions);
        return this;
    }

    public virtual IReadOnlySet<string> GetRequiredPermissions()
    {
        return _requiredPermissions;
    }

    protected virtual SettingPageContributorBase RequiredFeatures(params string[] features)
    {
        Check.NotNullOrEmpty(features, nameof(features));
        _requiredFeatures.AddIfNotContains(features);
        return this;
    }

    protected virtual SettingPageContributorBase RequiredTenantSideFeatures(params string[] features)
    {
        Check.NotNullOrEmpty(features, nameof(features));
        _requiredTenantSideFeatures.AddIfNotContains(features);
        return this;
    }

    public virtual IReadOnlySet<string> GetRequiredFeatures(MultiTenancySides? multiTenancySides = null)
    {
        return multiTenancySides == null || multiTenancySides != MultiTenancySides.Tenant
            ? _requiredFeatures
            : _requiredTenantSideFeatures;
    }

    public abstract Task ConfigureAsync(SettingPageCreationContext context);

    public virtual Task<bool> CheckPermissionsAsync(SettingPageCreationContext context)
    {
        return Task.FromResult(true);
    }
}
