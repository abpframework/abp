using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Modularity;
using Volo.Abp.Uow;
using Xunit;

namespace Volo.Abp.TenantManagement;

public abstract class LazyLoad_Tests<TStartupModule> : TenantManagementTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    public ITenantRepository TenantRepository { get; }

    protected LazyLoad_Tests()
    {
        TenantRepository = GetRequiredService<ITenantRepository>();
    }

    [Fact]
    public async Task Should_Lazy_Load_Tenant_Collections()
    {
        using (var uow = GetRequiredService<IUnitOfWorkManager>().Begin())
        {
            var role = await TenantRepository.FindByNameAsync("acme", includeDetails: false);
            role.ConnectionStrings.ShouldNotBeNull();
            role.ConnectionStrings.Any().ShouldBeTrue();

            await uow.CompleteAsync();
        }
    }
}
