using System;
using Microsoft.Extensions.DependencyInjection;
using Riok.Mapperly.Abstractions;
using Shouldly;
using Volo.Abp.Data;
using Volo.Abp.Mapperly.SampleClasses;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Testing;
using Xunit;

namespace Volo.Abp.Mapperly;

public class MyReverseClass
{
    public string Id { get; set; }

    public string Name { get; set; }
}

public class MyReverseClassDto
{
    public string Id { get; set; }

    public string Name { get; set; }
}

[Mapper]
public partial class MyReverseClassMapper : TwoWayMapperBase<MyReverseClass, MyReverseClassDto>
{
    public override partial MyReverseClassDto Map(MyReverseClass source);

    public override partial void Map(MyReverseClass source, MyReverseClassDto destination);

    public override partial MyReverseClass ReverseMap(MyReverseClassDto source);

    public override partial void ReverseMap(MyReverseClassDto source, MyReverseClass destination);

    public override void BeforeReverseMap(MyReverseClassDto source)
    {
        source.Name = "BeforeReverseMap " + source.Name;
    }

    public override void AfterReverseMap(MyReverseClassDto source, MyReverseClass destination)
    {
        destination.Name = source.Name + " AfterReverseMap";
    }
}

public class AbpReverseMapperly_Tests : AbpIntegratedTest<MapperlyTestModule>
{
    private readonly IObjectMapper _objectMapper;

    public AbpReverseMapperly_Tests()
    {
        _objectMapper = ServiceProvider.GetRequiredService<IObjectMapper>();
    }

    [Fact]
    public void Map_Test()
    {
        var myClass = new MyReverseClass { Id = "1", Name = "Test" };
        var myClassDto = _objectMapper.Map<MyReverseClass, MyReverseClassDto>(myClass);
        myClassDto.Name.ShouldBe("Test");

        myClass.Id = "2";
        myClass.Name = "Test2";

        _objectMapper.Map<MyReverseClass, MyReverseClassDto>(myClass, myClassDto);

        myClassDto.Id.ShouldBe("2");
        myClassDto.Name.ShouldBe("Test2");
    }

    [Fact]
    public void ReverseMap_Test()
    {
        var myClassDto = new MyReverseClassDto { Id = "1", Name = "Test" };
        var myClass = _objectMapper.Map<MyReverseClassDto, MyReverseClass>(myClassDto);
        myClass.Name.ShouldBe("BeforeReverseMap Test AfterReverseMap");

        myClassDto.Id = "2";
        myClassDto.Name = "Test2";

        _objectMapper.Map<MyReverseClassDto, MyReverseClass>(myClassDto, myClass);

        myClass.Id.ShouldBe("2");
        myClass.Name.ShouldBe("BeforeReverseMap Test2 AfterReverseMap");
    }

    [Fact]
    public void MapExtraProperties_Should_Filter_With_Single_Parameter_ReverseMap()
    {
        var dto = new ExtensibleReverseDto { Id = Guid.NewGuid() }
            .SetProperty("Tag", "ok")
            .SetProperty("Secret", "leaked");

        var entity = _objectMapper.Map<ExtensibleReverseDto, ExtensibleReverseEntity>(dto);

        entity.GetProperty<string>("Tag").ShouldBe("ok");
        entity.HasProperty("Secret").ShouldBeFalse();
    }
}
