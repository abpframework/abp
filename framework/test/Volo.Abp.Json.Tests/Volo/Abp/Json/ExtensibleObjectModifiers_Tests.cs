using Shouldly;
using Volo.Abp.Json.SystemTextJson;
using Volo.Abp.ObjectExtending;
using Xunit;

namespace Volo.Abp.Json;

public class ExtensibleObjectModifiers_Tests : AbpJsonTestBase
{
    [Fact]
    public void Should_Modify_Object()
    {
        var jsonSerializer = GetRequiredService<AbpSystemTextJsonSerializerProvider>();

        var extensibleObject = jsonSerializer.Deserialize<ExtensibleObject>("{\"ExtraProperties\": {\"Test-Key\":\"Test-Value\"}}");

        extensibleObject.ExtraProperties.ShouldContainKeyAndValue("Test-Key", "Test-Value");
    }
}
