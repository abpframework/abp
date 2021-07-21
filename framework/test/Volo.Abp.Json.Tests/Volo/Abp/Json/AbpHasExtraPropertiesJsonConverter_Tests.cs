using System.Collections.Generic;
using System.Linq;
using Shouldly;
using Volo.Abp.Data;
using Volo.Abp.ObjectExtending;
using Xunit;

namespace Volo.Abp.Json
{
    public class AbpHasExtraPropertiesJsonConverter_Tests: AbpJsonTestBase
    {
        private readonly IJsonSerializer _jsonSerializer;

        public AbpHasExtraPropertiesJsonConverter_Tests()
        {
            _jsonSerializer = GetRequiredService<IJsonSerializer>();
        }

        [Fact]
        public void JsonConverter_Test()
        {
            var fooDto = new FooDto
            {
                Name = "foo-dto",
                BarDtos = new List<BarDto>()
            };
            fooDto.SetProperty("foo", "foo-value");

            var barDto = new BarDto
            {
                Name = "bar-dto"
            };
            barDto.SetProperty("bar", "bar-value");
            fooDto.BarDtos.Add(barDto);

            var json = _jsonSerializer.Serialize(fooDto);

            fooDto = _jsonSerializer.Deserialize<FooDto>(json);
            fooDto.ShouldNotBeNull();
            fooDto.Name.ShouldBe("foo-dto");
            fooDto.GetProperty("foo").ShouldBe("foo-value");

            fooDto.BarDtos.Count.ShouldBe(1);
            fooDto.BarDtos.First().Name.ShouldBe("bar-dto");
            fooDto.BarDtos.First().GetProperty("bar").ShouldBe("bar-value");
        }
    }

    public class FooDto : ExtensibleObject
    {
        public string Name { get; set; }

        public List<BarDto> BarDtos { get; set; }
    }

    public class BarDto : ExtensibleObject
    {
        public string Name { get; set; }
    }
}
