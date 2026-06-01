using System.Collections.Generic;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Authorization.Permissions.Resources;
using Volo.Abp.Authorization.TestServices.Resources;
using Xunit;

namespace Volo.Abp.Authorization;

public class ResourcePermissionPopulator_Tests : AuthorizationTestBase
{
    private readonly ResourcePermissionPopulator _resourcePermissionPopulator;

    public ResourcePermissionPopulator_Tests()
    {
        _resourcePermissionPopulator = GetRequiredService<ResourcePermissionPopulator>();
    }

    [Fact]
    public async Task PopulateAsync()
    {
        var testResourceObject = new TestEntityResource(TestEntityResource.ResourceKey5);
        testResourceObject.ResourcePermissions.IsNullOrEmpty().ShouldBeTrue();

        await _resourcePermissionPopulator.PopulateAsync<TestEntityResource>(
            testResourceObject,
            TestEntityResource.ResourceName
        );

        testResourceObject.ResourcePermissions.ShouldNotBeNull();
        testResourceObject.ResourcePermissions.Count.ShouldBe(8);
        testResourceObject.ResourcePermissions["MyResourcePermission1"].ShouldBe(false);
        testResourceObject.ResourcePermissions["MyResourcePermission2"].ShouldBe(false);
        testResourceObject.ResourcePermissions["MyResourcePermission3"].ShouldBe(true);
        testResourceObject.ResourcePermissions["MyResourcePermission4"].ShouldBe(false);
        testResourceObject.ResourcePermissions["MyResourcePermission5"].ShouldBe(true);
        testResourceObject.ResourcePermissions["MyResourcePermission6"].ShouldBe(false);
        testResourceObject.ResourcePermissions["MyResourcePermission7"].ShouldBe(false);
        testResourceObject.ResourcePermissions["MyResourcePermission8"].ShouldBe(false);

        testResourceObject = new TestEntityResource(TestEntityResource.ResourceKey6);
        testResourceObject.ResourcePermissions.IsNullOrEmpty().ShouldBeTrue();

        await _resourcePermissionPopulator.PopulateAsync<TestEntityResource>(
            testResourceObject,
            TestEntityResource.ResourceName
        );

        testResourceObject.ResourcePermissions.ShouldNotBeNull();
        testResourceObject.ResourcePermissions.Count.ShouldBe(8);
        testResourceObject.ResourcePermissions["MyResourcePermission1"].ShouldBe(false);
        testResourceObject.ResourcePermissions["MyResourcePermission2"].ShouldBe(false);
        testResourceObject.ResourcePermissions["MyResourcePermission3"].ShouldBe(false);
        testResourceObject.ResourcePermissions["MyResourcePermission4"].ShouldBe(false);
        testResourceObject.ResourcePermissions["MyResourcePermission5"].ShouldBe(false);
        testResourceObject.ResourcePermissions["MyResourcePermission6"].ShouldBe(true);
        testResourceObject.ResourcePermissions["MyResourcePermission7"].ShouldBe(false);
        testResourceObject.ResourcePermissions["MyResourcePermission8"].ShouldBe(false);
    }
}
