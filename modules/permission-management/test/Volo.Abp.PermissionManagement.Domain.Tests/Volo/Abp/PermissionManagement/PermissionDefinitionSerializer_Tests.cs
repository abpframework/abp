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
        permissionGroupRecord.DisplayName.ShouldBe("Group one");
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
                new FixedLocalizableString("Permission one"),
                MultiTenancySides.Tenant
            )
            .WithProviders("ProviderA", "ProviderB")
            .WithProperty("CustomProperty2", "CustomValue2")
            .RequireGlobalFeatures("GlobalFeature1", "GlobalFeature2");

        // Act
        
        var permissionRecord = await _serializer.SerializeAsync(
            permission1,
            group1
        );
        
        //Assert
        
        permissionRecord.Name.ShouldBe("Permission1");
        permissionRecord.GroupName.ShouldBe("Group1");
        permissionRecord.DisplayName.ShouldBe("Permission one");
        permissionRecord.GetProperty("CustomProperty2").ShouldBe("CustomValue2");
        permissionRecord.Providers.ShouldBe("ProviderA,ProviderB");
        permissionRecord.MultiTenancySide.ShouldBe(MultiTenancySides.Tenant);
        permissionRecord.StateCheckers.ShouldBe("[{\"T\":\"GF\",\"A\":true,\"N\":[\"GlobalFeature1\",\"GlobalFeature2\"]}]");
    }
    
    [Fact(Skip = "Not implemented yet")]
    public async Task Deserialize_Complex_Permission_Definition()
    {
        // Arrange
      
        var permissionRecord = new PermissionDefinitionRecord
        {
            Name = "Permission1",
            GroupName = "Group1",
            DisplayName = "Permission one",
            Providers = "ProviderA,ProviderB",
            IsEnabled = true,
            MultiTenancySide = MultiTenancySides.Tenant,
            StateCheckers = "[{\"T\":\"GF\",\"A\":true,\"N\":[\"GlobalFeature1\",\"GlobalFeature2\"]}]"
        };

        // Act

        var permission = await _serializer.DeserializeAsync(permissionRecord);
       
        //Assert

        permission.Name.ShouldBe("Permission1");
        permission.DisplayName.ShouldBeOfType<FixedLocalizableString>();
        permission.DisplayName.As<FixedLocalizableString>().Value.ShouldBe("Permission one");
        permission.Parent.ShouldBeNull();
        permission.MultiTenancySide.ShouldBe(MultiTenancySides.Tenant);
        permission.IsEnabled.ShouldBe(true);
        permission.Providers.Count.ShouldBe(2);
        permission.Providers.ShouldContain("ProviderA");
        permission.Providers.ShouldContain("ProviderB");
        permission.StateCheckers.Count.ShouldBe(1);
        permission.StateCheckers[0].ShouldBeOfType<RequireGlobalFeaturesSimpleStateChecker<PermissionDefinition>>();
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