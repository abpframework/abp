using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Json.SystemTextJson;
using Volo.Abp.Json.SystemTextJson.Modifiers;
using Xunit;

namespace Volo.Abp.Json;

public class AbpIncludeNonPublicPropertiesModifiers_Tests : AbpJsonTestBase
{
    private readonly IJsonSerializer _jsonSerializer;

    public AbpIncludeNonPublicPropertiesModifiers_Tests()
    {
        _jsonSerializer = GetRequiredService<IJsonSerializer>();
    }

    protected override void AfterAddApplication(IServiceCollection services)
    {
        services.Configure<AbpSystemTextJsonSerializerModifiersOptions>(options =>
        {
            options.Modifiers.Add(new AbpIncludeNonPublicPropertiesModifiers<NonPublicPropertiesClass, string>().CreateModifyAction(x => x.Name));
            options.Modifiers.Add(new AbpIncludeNonPublicPropertiesModifiers<NonPublicPropertiesClass, string>().CreateModifyAction(x => x.Age));
        });

        base.AfterAddApplication(services);
    }

    [Fact]
    public void Test()
    {
        var json = _jsonSerializer.Serialize(new NonPublicPropertiesClass()
        {
            Id = "id"
        });

        json.ShouldContain("id");
        json.ShouldContain("name");
        json.ShouldContain("age");

        var obj = _jsonSerializer.Deserialize<NonPublicPropertiesClass>(json);
        obj.Id.ShouldBe("id");
        obj.Name.ShouldBe("name");
        obj.Age.ShouldBe("age");
    }

    class NonPublicPropertiesClass
    {
        public string Id { get; set; }

        public string Name { get; private set; }

        public string Age { get; protected set; }

        public NonPublicPropertiesClass()
        {
            Name = "name";
            Age = "age";
        }
    }
}
