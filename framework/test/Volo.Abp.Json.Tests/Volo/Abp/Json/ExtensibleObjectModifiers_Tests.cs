using Shouldly;
using Volo.Abp.Data;
using Volo.Abp.Json.SystemTextJson;
using Volo.Abp.ObjectExtending;
using Xunit;

namespace Volo.Abp.Json;

public class ExtensibleObjectModifiers_Tests : AbpJsonSystemTextJsonTestBase
{
    [Fact]
    public void Should_Modify_Object()
    {
        var jsonSerializer = GetRequiredService<AbpSystemTextJsonSerializer>();

        var extensibleObject = jsonSerializer.Deserialize<ExtensibleObject>("{\"ExtraProperties\": {\"Test-Key\":\"Test-Value\"}}");
        extensibleObject.ExtraProperties.ShouldContainKeyAndValue("Test-Key", "Test-Value");

        var bar = jsonSerializer.Deserialize<Bar>("{\"ExtraProperties\": {\"Test-Key\":\"Test-Value\"}}");
        bar.ExtraProperties.ShouldContainKeyAndValue("Test-Key", "Test-Value");
    }
}

public abstract class Foo : IHasExtraProperties
{
    public ExtraPropertyDictionary ExtraProperties { get; protected set; }
}

public class Bar : Foo
{

}
