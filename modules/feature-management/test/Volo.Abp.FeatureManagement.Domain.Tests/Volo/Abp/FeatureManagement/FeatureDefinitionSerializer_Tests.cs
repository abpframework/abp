using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Data;
using Volo.Abp.FeatureManagement.Localization;
using Volo.Abp.Features;
using Volo.Abp.Localization;
using Volo.Abp.Validation.StringValues;
using Xunit;

namespace Volo.Abp.FeatureManagement;

public class FeatureDefinitionSerializer_Tests : FeatureManagementDomainTestBase
{
    private readonly IFeatureDefinitionSerializer _serializer;

    public FeatureDefinitionSerializer_Tests()
    {
        _serializer = GetRequiredService<IFeatureDefinitionSerializer>();
    }

    [Fact]
    public async Task Serialize_Feature_Group_Definition()
    {
        // Arrange

        var context = new FeatureDefinitionContext();
        var group1 = CreateFeatureGroup1(context);

        // Act

        var featureGroupRecord = await _serializer.SerializeAsync(group1);

        //Assert

        featureGroupRecord.Name.ShouldBe("Group1");
        featureGroupRecord.DisplayName.ShouldBe("F:Group one");
        featureGroupRecord.GetProperty("CustomProperty1").ShouldBe("CustomValue1");
    }

    [Fact]
    public async Task Serialize_Complex_Feature_Definition()
    {
        // Arrange

        var context = new FeatureDefinitionContext();
        var group1 = CreateFeatureGroup1(context);
        var feature1 = group1.AddFeature(
                "Feature1",
                "default",
                new LocalizableString(typeof(AbpFeatureManagementResource), "Feature1"),
                new LocalizableString(typeof(AbpFeatureManagementResource), "Feature1"),
                new ToggleStringValueType(),
                isVisibleToClients: true
            )
            .WithProviders("ProviderA", "ProviderB")
            .WithProperty("CustomProperty2", "CustomValue2");

        // Act

        var featureRecord = await _serializer.SerializeAsync(
            feature1,
            group1
        );

        //Assert

        featureRecord.Name.ShouldBe("Feature1");
        featureRecord.GroupName.ShouldBe("Group1");
        featureRecord.DisplayName.ShouldBe("L:AbpFeatureManagement,Feature1");
        featureRecord.ValueType.ShouldBe("{\"name\":\"ToggleStringValueType\",\"properties\":{},\"validator\":{\"name\":\"BOOLEAN\",\"properties\":{}}}");
        featureRecord.GetProperty("CustomProperty2").ShouldBe("CustomValue2");
        featureRecord.AllowedProviders.ShouldBe("ProviderA,ProviderB");
    }

    private static FeatureGroupDefinition CreateFeatureGroup1(
        IFeatureDefinitionContext context)
    {
        var group = context.AddGroup(
            "Group1",
            displayName: new FixedLocalizableString("Group one")
        );

        group["CustomProperty1"] = "CustomValue1";

        return group;
    }
}
