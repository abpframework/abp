using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Mapperly.SampleClasses;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Testing;
using Xunit;

namespace Volo.Abp.Mapperly;

public class AbpMapperlyModule_Basic_Tests : AbpIntegratedTest<MapperlyTestModule>
{
    private readonly IObjectMapper _objectMapper;

    public AbpMapperlyModule_Basic_Tests()
    {
        _objectMapper = ServiceProvider.GetRequiredService<IObjectMapper>();
    }

    [Fact]
    public void Should_Replace_IAutoObjectMappingProvider()
    {
        Assert.True(ServiceProvider.GetRequiredService<IAutoObjectMappingProvider>() is MapperlyAutoObjectMappingProvider);
    }

    [Fact]
    public void Should_Map_Objects_With_AutoMap_Attributes()
    {
        var dto = _objectMapper.Map<MyEntity, MyEntityDto>(new MyEntity { Number = 42 });
        dto.Number.ShouldBe(42);
    }

    [Fact]
    public void Should_Map_Objects_With_Existing_Target_Object()
    {
        var dto = new MyEntityDto {Id = Guid.Empty, Number = 42};

        _objectMapper.Map<MyEntity, MyEntityDto>(new MyEntity { Id = Guid.NewGuid(), Number = 43 }, dto);

        dto.Number.ShouldBe(43);
        dto.Id.ShouldNotBe(Guid.Empty);
    }

    [Fact]
    public void Should_Map_Collection()
    {
        var dto = _objectMapper.Map<List<MyEntity>, List<MyEntityDto>>(new List<MyEntity>
        {
            new MyEntity { Number = 42 },
            new MyEntity { Number = 43 }
        });

        dto.Count.ShouldBe(2);
        dto[0].Number.ShouldBe(42);
        dto[1].Number.ShouldBe(43);

        var dto2 = _objectMapper.Map<IReadOnlyList<MyEntity>, MyEntityDto[]>(new List<MyEntity>
        {
            new MyEntity { Number = 42 },
            new MyEntity { Number = 43 }
        }.AsReadOnly());

        dto2.Length.ShouldBe(2);
        dto2[0].Number.ShouldBe(42);
        dto2[1].Number.ShouldBe(43);

        var dtoList = new List<MyEntityDto>();
        {
            new MyEntityDto() { Number = 44 };
            new MyEntityDto() { Number = 45 };
        }

        _objectMapper.Map<List<MyEntity>, List<MyEntityDto>>(new List<MyEntity>
        {
            new MyEntity { Number = 42 },
            new MyEntity { Number = 43 }
        }, dtoList);

        dtoList.Count.ShouldBe(2);
        dtoList[0].Number.ShouldBe(42);
        dtoList[1].Number.ShouldBe(43);

        var dtoArray = dtoList.ToArray();
        _objectMapper.Map<IReadOnlyList<MyEntity>, MyEntityDto[]>(new List<MyEntity>
        {
            new MyEntity { Number = 42 },
            new MyEntity { Number = 43 }
        }.AsReadOnly(), dtoArray);

        dtoArray.Length.ShouldBe(2);
        dtoArray[0].Number.ShouldBe(42);
        dtoArray[1].Number.ShouldBe(43);
    }

    [Fact]
    public void Should_Map_Enum()
    {
        var dto = _objectMapper.Map<MyEnum, MyEnumDto>(MyEnum.Value3);
        dto.ShouldBe(MyEnumDto.Value2); //Value2 is same as Value3
    }

    [Fact]
    public void Should_Throw_Exception_If_Mapper_Is_Not_Found()
    {
        var exception = Assert.Throws<AbpException>(() =>_objectMapper.Map<MyEntity, MyClassDto>(new MyEntity()));
        exception.Message.ShouldBe("No object mapping was found for the specified source and destination types.\n\n" +
                                   "Mapping attempted:\n" +
                                   "MyEntity -> MyClassDto\n" +
                                   "Volo.Abp.Mapperly.SampleClasses.MyEntity -> Volo.Abp.Mapperly.MyClassDto\n\n" +
                                   "How to fix:\n" +
                                   "Define a mapping class for these types:" + "\n" +
                                   "   - Use MapperBase<TSource, TDestination> for one-way mapping.\n" +
                                   "   - Use TwoWayMapperBase<TDestination, TSource> for two-way mapping.\n\n" +
                                   "For details, see the Mapperly integration document https://abp.io/docs/latest/framework/infrastructure/object-to-object-mapping#mapperly-integration");
    }
}
