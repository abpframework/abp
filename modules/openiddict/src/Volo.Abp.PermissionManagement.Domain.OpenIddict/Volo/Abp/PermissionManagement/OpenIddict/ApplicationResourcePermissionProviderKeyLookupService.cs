using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Authorization.Permissions.Resources;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Localization;
using Volo.Abp.OpenIddict.Applications;
using Volo.Abp.OpenIddict.Localization;

namespace Volo.Abp.PermissionManagement.OpenIddict;

public class ApplicationResourcePermissionProviderKeyLookupService : IResourcePermissionProviderKeyLookupService, ITransientDependency
{
    public string Name => ClientResourcePermissionValueProvider.ProviderName;

    public ILocalizableString DisplayName { get; }

    protected IApplicationFinder ApplicationFinder { get; }

    public ApplicationResourcePermissionProviderKeyLookupService(IApplicationFinder applicationFinder)
    {
        ApplicationFinder = applicationFinder;
        DisplayName = LocalizableString.Create<AbpOpenIddictResource>(nameof(ApplicationResourcePermissionProviderKeyLookupService));
    }

    public virtual async Task<List<ResourcePermissionProviderKeyInfo>> SearchAsync(string filter = null, int page = 1, CancellationToken cancellationToken = default)
    {
        var applications = await ApplicationFinder.SearchAsync(filter, page);
        return applications.Select(x => new ResourcePermissionProviderKeyInfo(x.ClientId, x.ClientId)).ToList();
    }

    public virtual Task<List<ResourcePermissionProviderKeyInfo>> SearchAsync(string[] keys, CancellationToken cancellationToken = default)
    {
        // Keys are ClientIds
        return Task.FromResult(keys.Select(x => new ResourcePermissionProviderKeyInfo(x, x)).ToList());
    }
}
