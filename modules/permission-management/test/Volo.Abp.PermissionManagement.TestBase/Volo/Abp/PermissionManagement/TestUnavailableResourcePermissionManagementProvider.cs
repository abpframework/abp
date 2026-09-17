using System.Threading.Tasks;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.PermissionManagement;

public class TestUnavailableResourcePermissionManagementProvider : ResourcePermissionManagementProvider
{
    public override string Name => "TestUnavailable";

    public TestUnavailableResourcePermissionManagementProvider(
        IResourcePermissionGrantRepository resourcePermissionGrantRepository,
        IGuidGenerator guidGenerator,
        ICurrentTenant currentTenant)
        : base(
            resourcePermissionGrantRepository,
            guidGenerator,
            currentTenant)
    {
    }

    public override Task<bool> IsAvailableAsync()
    {
        return Task.FromResult(false);
    }
}
