using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Features;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.SettingManagement.Web.Pages.SettingManagement;

public class SettingPageContributorManager : IScopedDependency
{
    protected IServiceProvider ServiceProvider  { get; }

    protected ConcurrentDictionary<string, List<ISettingPageContributor>> CachedAvailableContributors { get; set; }

    public SettingPageContributorManager(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
        CachedAvailableContributors = new ConcurrentDictionary<string, List<ISettingPageContributor>>();
    }

    public virtual async Task<List<ISettingPageContributor>> GetAvailableContributors()
    {
        return CachedAvailableContributors.GetOrAdd(nameof(SettingPageContributorManager), await GetAvailableContributorsInternalAsync());
    }

    public virtual async Task<SettingPageCreationContext> ConfigureAsync()
    {
        var contributors = CachedAvailableContributors.GetOrAdd(nameof(SettingPageContributorManager), await GetAvailableContributorsInternalAsync());
        var context = new SettingPageCreationContext(ServiceProvider);
        foreach (var contributor in contributors)
        {
            await contributor.ConfigureAsync(context);
        }
        context.Normalize();
        return context;
    }

    protected virtual async Task<List<ISettingPageContributor>> GetAvailableContributorsInternalAsync()
    {
        var contributors = ServiceProvider.GetRequiredService<IOptions<SettingManagementPageOptions>>().Value.Contributors;
        var contributorBases = contributors.Where(x => x is SettingPageContributorBase).Cast<SettingPageContributorBase>().ToList();

        var permissions = new HashSet<string>();
        permissions.AddIfNotContains(contributorBases.SelectMany(x => x.GetRequiredPermissions()));

        var featureChecker = ServiceProvider.GetRequiredService<IFeatureChecker>();
        var permissionChecker = ServiceProvider.GetRequiredService<IPermissionChecker>();
        var grantResult = await permissionChecker.IsGrantedAsync(permissions.ToArray());

        var availableContributors = new List<ISettingPageContributor>();
        foreach (var contributor in contributorBases)
        {
            var available = true;

            var requiredFeatures = contributor.GetRequiredFeatures();
            if (requiredFeatures.Any() && !await featureChecker.IsEnabledAsync(true, requiredFeatures.ToArray()))
            {
                available = false;
            }

            if (available)
            {
                var requiredTenantSideFeatures = contributor.GetRequiredFeatures(MultiTenancySides.Tenant);
                if (requiredTenantSideFeatures.Any() &&
                    ServiceProvider.GetRequiredService<ICurrentTenant>().IsAvailable &&
                    !await featureChecker.IsEnabledAsync(true, requiredTenantSideFeatures.ToArray()))
                {
                    available = false;
                }
            }

            if (available)
            {
                available = contributor.GetRequiredPermissions().All(x => grantResult.Result.ContainsKey(x) && grantResult.Result[x] == PermissionGrantResult.Granted);
            }

            if (available)
            {
                availableContributors.Add(contributor);
            }
        }

        var context = new SettingPageCreationContext(ServiceProvider);
        foreach (var contributor in contributors.Where(x => x is not SettingPageContributorBase))
        {
#pragma warning disable CS0618
            if (await contributor.CheckPermissionsAsync(context))
#pragma warning restore CS0618
            {
                availableContributors.Add(contributor);
            }
        }

        return availableContributors;
    }
}
