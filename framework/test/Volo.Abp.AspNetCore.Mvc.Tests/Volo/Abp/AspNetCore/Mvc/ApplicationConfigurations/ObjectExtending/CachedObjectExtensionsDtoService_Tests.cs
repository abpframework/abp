using Shouldly;
using Volo.Abp.ObjectExtending;
using Volo.Abp.ObjectExtending.Modularity;
using Xunit;

namespace Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations.ObjectExtending;

public class CachedObjectExtensionsDtoService_Tests
{
    private const string ModuleName = "CachedObjectExtensionsDtoServiceTestModule";
    private const string EntityName = "TestEntity";

    [Fact]
    public void Should_Keep_The_Feature_And_Global_Feature_Policies_Apart()
    {
        ObjectExtensionManager.Instance.Modules()
            .ConfigureModule<ModuleExtensionConfiguration>(ModuleName, module =>
            {
                module.ConfigureEntity(EntityName, entity =>
                {
                    entity.AddOrUpdateProperty<string>("TestProperty", property =>
                    {
                        property.Policy.Features.Features = ["TestFeature"];
                        property.Policy.Features.RequiresAll = true;
                        property.Policy.GlobalFeatures.Features = ["TestGlobalFeature"];
                        property.Policy.GlobalFeatures.RequiresAll = false;
                    });
                });
            });

        var service = new CachedObjectExtensionsDtoService(new ExtensionPropertyAttributeDtoFactory());

        var policy = service.Get()
            .Modules[ModuleName]
            .Entities[EntityName]
            .Properties["TestProperty"]
            .Policy;

        policy.Features.Features.ShouldBe(["TestFeature"]);
        policy.Features.RequiresAll.ShouldBeTrue();
        policy.GlobalFeatures.Features.ShouldBe(["TestGlobalFeature"]);
        policy.GlobalFeatures.RequiresAll.ShouldBeFalse();
    }
}
