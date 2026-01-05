using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Authorization.Permissions.Resources;
using Xunit;

namespace Volo.Abp.PermissionManagement;

public class ResourcePermissionChecker_Basic_Tests : PermissionTestBase
{
    private readonly IResourcePermissionChecker _resourcePermissionChecker;

    public ResourcePermissionChecker_Basic_Tests()
    {
        _resourcePermissionChecker = GetRequiredService<IResourcePermissionChecker>();
    }

    [Fact]
    public async Task Should_Return_Prohibited_If_Permission_Is_Not_Defined()
    {
        (await _resourcePermissionChecker.IsGrantedAsync(TestEntityResource.ResourceName, TestEntityResource.ResourceKey1,"UndefinedResourcePermissionName")).ShouldBeFalse();
    }

    [Fact]
    public async Task Should_Return_False_As_Default_For_Any_Permission()
    {
        (await _resourcePermissionChecker.IsGrantedAsync(TestEntityResource.ResourceName, TestEntityResource.ResourceKey1,"MyPermission1")).ShouldBeFalse();
    }
}
