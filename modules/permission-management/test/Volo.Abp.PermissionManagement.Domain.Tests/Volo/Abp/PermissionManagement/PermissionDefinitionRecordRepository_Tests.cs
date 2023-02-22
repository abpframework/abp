using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Modularity;
using Xunit;

namespace Volo.Abp.PermissionManagement;

public abstract class PermissionDefinitionRecordRepository_Tests<TStartupModule> : PermissionManagementTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    protected IPermissionDefinitionRecordRepository PermissionDefinitionRecordRepository { get; set; }

    protected PermissionDefinitionRecordRepository_Tests()
    {
        PermissionDefinitionRecordRepository = GetRequiredService<IPermissionDefinitionRecordRepository>();
    }

    [Fact]
    public async Task FindByNameAsync()
    {
        var permission = await PermissionDefinitionRecordRepository.FindByNameAsync("MyPermission1");
        permission.ShouldNotBeNull();
        permission.Name.ShouldBe("MyPermission1");

        permission = await PermissionDefinitionRecordRepository.FindByNameAsync("MyPermission2");
        permission.ShouldNotBeNull();
        permission.Name.ShouldBe("MyPermission2");
    }
}
