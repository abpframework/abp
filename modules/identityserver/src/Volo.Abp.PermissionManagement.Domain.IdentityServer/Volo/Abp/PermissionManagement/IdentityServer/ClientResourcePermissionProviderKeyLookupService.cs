using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Authorization.Permissions.Resources;
using Volo.Abp.DependencyInjection;
using Volo.Abp.IdentityServer.Clients;
using Volo.Abp.IdentityServer.Localization;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.PermissionManagement.IdentityServer;

public class ClientResourcePermissionProviderKeyLookupService : IResourcePermissionProviderKeyLookupService, ITransientDependency
{
    public string Name => ClientResourcePermissionValueProvider.ProviderName;

    public ILocalizableString DisplayName { get; }

    protected IClientFinder ClientFinder { get; }

    protected ICurrentTenant CurrentTenant { get; }

    public ClientResourcePermissionProviderKeyLookupService(
        IClientFinder clientFinder,
        ICurrentTenant currentTenant)
    {
        ClientFinder = clientFinder;
        CurrentTenant = currentTenant;
        DisplayName = LocalizableString.Create<AbpIdentityServerResource>(nameof(ClientResourcePermissionProviderKeyLookupService));
    }

    public virtual Task<bool> IsAvailableAsync()
    {
        return Task.FromResult(CurrentTenant.Id == null);
    }

    public virtual async Task<List<ResourcePermissionProviderKeyInfo>> SearchAsync(string filter = null, int page = 1, CancellationToken cancellationToken = default)
    {
        var clients = await ClientFinder.SearchAsync(filter, page);
        return clients.Select(x => new ResourcePermissionProviderKeyInfo(x.ClientId, x.ClientId)).ToList();
    }

    public virtual Task<List<ResourcePermissionProviderKeyInfo>> SearchAsync(string[] keys, CancellationToken cancellationToken = default)
    {
        // Keys are ClientIds
        return Task.FromResult(keys.Select(x => new ResourcePermissionProviderKeyInfo(x, x)).ToList());
    }
}
