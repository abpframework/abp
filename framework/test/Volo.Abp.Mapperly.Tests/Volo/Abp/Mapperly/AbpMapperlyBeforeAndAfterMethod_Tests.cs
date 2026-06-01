using Microsoft.Extensions.DependencyInjection;
using Riok.Mapperly.Abstractions;
using Shouldly;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Testing;
using Xunit;

namespace Volo.Abp.Mapperly;

public class MyClass
{
    public string Id { get; set; }

    public string Name { get; set; }
}

public class MyClassDto
{
    public string Id { get; set; }

    public string Name { get; set; }
}

[Mapper]
public partial class MyClassMapper : MapperBase<MyClass, MyClassDto>
{
    public override partial MyClassDto Map(MyClass source);

    public override partial void Map(MyClass source, MyClassDto destination);

    public override void BeforeMap(MyClass source)
    {
        source.Name = "BeforeMap " + source.Name;
    }

    public override void AfterMap(MyClass source, MyClassDto destination)
    {
        destination.Name = source.Name + " AfterMap";
    }
}

public class AbpMapperlyBeforeAndAfterMethod_Tests : AbpIntegratedTest<MapperlyTestModule>
{
    private readonly IObjectMapper _objectMapper;

    public AbpMapperlyBeforeAndAfterMethod_Tests()
    {
        _objectMapper = ServiceProvider.GetRequiredService<IObjectMapper>();
    }

    [Fact]
    public void BeforeAndAfterMethods_Should_Be_Called_When_Mapping()
    {
        var myClass = new MyClass { Id = "1", Name = "Test" };

        var myClassDto = _objectMapper.Map<MyClass, MyClassDto>(myClass);
        myClassDto.Name.ShouldBe("BeforeMap Test AfterMap");
    }
}
