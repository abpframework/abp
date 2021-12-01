using System.Collections.Generic;
using System.Linq;
using Shouldly;
using Volo.Abp.Data;
using Volo.Abp.ObjectExtending;
using Xunit;

namespace Volo.Abp.Json
{
    public class ExtensibleObject_Tests: AbpJsonTestBase
    {
        private readonly IJsonSerializer _jsonSerializer;

        public ExtensibleObject_Tests()
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

            fooDto.Name = "new-foo-dto";
            fooDto.SetProperty("foo", "new-foo-value");
            fooDto.BarDtos.First().Name = "new-bar-dto";
            fooDto.BarDtos.First().SetProperty("bar", "new-bar-value");

            json = _jsonSerializer.Serialize(fooDto);

            fooDto = _jsonSerializer.Deserialize<FooDto>(json);
            fooDto.ShouldNotBeNull();
            fooDto.Name.ShouldBe("new-foo-dto");
            fooDto.GetProperty("foo").ShouldBe("new-foo-value");

            fooDto.BarDtos.Count.ShouldBe(1);
            fooDto.BarDtos.First().Name.ShouldBe("new-bar-dto");
            fooDto.BarDtos.First().GetProperty("bar").ShouldBe("new-bar-value");
        }
        
        [Fact]
        public void SelfReference_Test()
        {
            var parentNodeDto = new NodeDto
            {
                Name = "parentNode",
            };
            parentNodeDto.SetProperty("node", "parent-value");

            var nodeDto = new NodeDto
            {
                Name = "node",
                Parent = parentNodeDto
            };
            nodeDto.SetProperty("node", "node-value");

            var json = _jsonSerializer.Serialize(nodeDto);

            nodeDto = _jsonSerializer.Deserialize<NodeDto>(json);
            nodeDto.ShouldNotBeNull();
            nodeDto.Name.ShouldBe("node");
            nodeDto.GetProperty("node").ShouldBe("node-value");

            nodeDto.Parent.ShouldNotBeNull();
            nodeDto.Parent.Name.ShouldBe("parentNode");
            nodeDto.Parent.GetProperty("node").ShouldBe("parent-value");
        }
    }

    class FooDto : ExtensibleObject
    {
        public string Name { get; set; }

        public List<BarDto> BarDtos { get; set; }
    }

    class BarDto : ExtensibleObject
    {
        public string Name { get; set; }
        
        public FooDto FooDto { get; set; }
    }
    
    class NodeDto : ExtensibleObject
    {
        public string Name { get; set; }
        
        public NodeDto Parent { get; set; }
    }
}
