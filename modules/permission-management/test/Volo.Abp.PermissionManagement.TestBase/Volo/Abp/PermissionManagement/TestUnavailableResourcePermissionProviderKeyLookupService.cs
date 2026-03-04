using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Localization;

namespace Volo.Abp.PermissionManagement;

public class TestUnavailableResourcePermissionProviderKeyLookupService : IResourcePermissionProviderKeyLookupService, ITransientDependency
{
    public string Name => "TestUnavailable";

    public ILocalizableString DisplayName => new LocalizableString("TestUnavailable", "TestResource");

    public Task<bool> IsAvailableAsync()
    {
        return Task.FromResult(false);
    }

    public Task<List<ResourcePermissionProviderKeyInfo>> SearchAsync(string filter = null, int page = 1, CancellationToken cancellationToken = default)
    {
        throw new System.NotImplementedException();
    }

    public Task<List<ResourcePermissionProviderKeyInfo>> SearchAsync(string[] keys, CancellationToken cancellationToken = default)
    {
        throw new System.NotImplementedException();
    }
}
