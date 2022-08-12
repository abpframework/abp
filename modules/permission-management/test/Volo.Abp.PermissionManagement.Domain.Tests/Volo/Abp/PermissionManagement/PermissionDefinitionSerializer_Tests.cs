using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Data;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;
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
    public async Task Serialize_Permission_Definitions()
    {
        // Arrange
        
        var context = new PermissionDefinitionContext(null);
        var group1 = CreatePermissionGroup1(context);
        var permission1 = group1.AddPermission(
            "Permission1",
            new FixedLocalizableString("Permission one"),
            MultiTenancySides.Tenant
        );
        
        permission1["CustomProperty2"] = "CustomValue2";

        // Act
        
        var permissionRecord = await _serializer.SerializeAsync(
            group1.GetPermissionOrNull("Permission1"),
            group1
        );
        
        //Assert
        
        permissionRecord.Name.ShouldBe("Permission1");
        permissionRecord.GroupName.ShouldBe("Group1");
        permissionRecord.DisplayName.ShouldBe("Permission one");
        permissionRecord.GetProperty("CustomProperty2").ShouldBe("CustomValue2");
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