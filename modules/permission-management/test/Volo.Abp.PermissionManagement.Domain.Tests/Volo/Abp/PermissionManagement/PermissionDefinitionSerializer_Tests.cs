using System;
using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Data;
using Volo.Abp.Features;
using Volo.Abp.GlobalFeatures;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;
using Volo.Abp.PermissionManagement.Localization;
using Xunit;

namespace Volo.Abp.PermissionManagement;

public class PermissionDefinitionSerializer_Tests : PermissionTestBase
{
    private readonly IPermissionDefinitionSerializer _serializer;
    
    public PermissionDefinitionSerializer_Tests()
    {
        _serializer = GetRequiredService<IPermissionDefinitionSerializer>();
    }

    [Fact]
    public async Task Serialize_Permission_Group_Definition()
    {
        // Arrange
        
        var context = new PermissionDefinitionContext(null);
        var group1 = CreatePermissionGroup1(context);
        
        // Act

        var permissionGroupRecord = await _serializer.SerializeAsync(group1);
            
        //Assert

        permissionGroupRecord.Name.ShouldBe("Group1");
        permissionGroupRecord.DisplayName.ShouldBe("F:Group one");
        permissionGroupRecord.GetProperty("CustomProperty1").ShouldBe("CustomValue1");
    }
    
    [Fact]
    public async Task Serialize_Complex_Permission_Definition()
    {
        // Arrange
        
        var context = new PermissionDefinitionContext(null);
        var group1 = CreatePermissionGroup1(context);
        var permission1 = group1.AddPermission(
                "Permission1",
                new LocalizableString(typeof(AbpPermissionManagementResource), "Permission1"),
                MultiTenancySides.Tenant
            )
            .WithProviders("ProviderA", "ProviderB")
            .WithProperty("CustomProperty2", "CustomValue2")
            .RequireAuthenticated() //For for testing, not so meaningful
            .RequireGlobalFeatures("GlobalFeature1", "GlobalFeature2")
            .RequireFeatures("Feature1", "Feature2")
            .RequirePermissions(requiresAll: false, batchCheck: false,"Permission2", "Permission3");

        // Act
        
        var permissionRecord = await _serializer.SerializeAsync(
            permission1,
            group1
        );
        
        //Assert
        
        permissionRecord.Name.ShouldBe("Permission1");
        permissionRecord.GroupName.ShouldBe("Group1");
        permissionRecord.DisplayName.ShouldBe("L:AbpPermissionManagement,Permission1");
        permissionRecord.GetProperty("CustomProperty2").ShouldBe("CustomValue2");
        permissionRecord.Providers.ShouldBe("ProviderA,ProviderB");
        permissionRecord.MultiTenancySide.ShouldBe(MultiTenancySides.Tenant);
        permissionRecord.StateCheckers.ShouldBe("[{\"T\":\"A\"},{\"T\":\"G\",\"A\":true,\"N\":[\"GlobalFeature1\",\"GlobalFeature2\"]},{\"T\":\"F\",\"A\":true,\"N\":[\"Feature1\",\"Feature2\"]},{\"T\":\"P\",\"A\":false,\"N\":[\"Permission2\",\"Permission3\"]}]");
    }

    private static PermissionGroupDefinition CreatePermissionGroup1(
        IPermissionDefinitionContext context)
    {
        var group = context.AddGroup(
            "Group1",
            displayName: new FixedLocalizableString("Group one")
        );
        
        group["CustomProperty1"] = "CustomValue1";
        
        return group;
    }
}