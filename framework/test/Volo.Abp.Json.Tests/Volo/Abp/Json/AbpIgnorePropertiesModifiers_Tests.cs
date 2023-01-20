using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Json.SystemTextJson;
using Volo.Abp.Json.SystemTextJson.Modifiers;
using Xunit;

namespace Volo.Abp.Json;

public class AbpIgnorePropertiesModifiers_Tests : AbpJsonTestBase
{
    private readonly IJsonSerializer _jsonSerializer;

    public AbpIgnorePropertiesModifiers_Tests()
    {
        _jsonSerializer = GetRequiredService<IJsonSerializer>();
    }

    protected override void AfterAddApplication(IServiceCollection services)
    {
        services.Configure<AbpSystemTextJsonSerializerModifiersOptions>(options =>
        {
            options.Modifiers.Add(new AbpIgnorePropertiesModifiers<FooDto, List<BarDto>>().CreateModifyAction(x => x.BarDtos));
            options.Modifiers.Add(new AbpIgnorePropertiesModifiers<BarDto, string>().CreateModifyAction(x => x.Id));
        });

        base.AfterAddApplication(services);
    }

    [Fact]
    public void Test()
    {
        var json = _jsonSerializer.Serialize(new FooDto()
        {
            Name = "foo",
            BarDtos = new List<BarDto>
            {
                new BarDto
                {
                    Name = "bar1"
                },
                new BarDto
                {
                    Name = "bar2"
                }
            }
        });

        json.ShouldNotContain("bar");

        json = _jsonSerializer.Serialize(new BarDto()
        {
            Id = "id",
            Name = "bar"
        });

        json.ShouldNotContain("id");
    }

    class FooDto
    {
        public string Name { get; set; }

        public List<BarDto> BarDtos { get; set; }
    }

    class BarDto
    {
        public string Id { get; set; }

        public string Name { get; set; }
    }
}
