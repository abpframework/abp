using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.AutoMapper.SampleClasses;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Testing;
using Xunit;

namespace Volo.Abp.AutoMapper;

public class AbpAutoMapperModule_Specific_ObjectMapper_Tests : AbpIntegratedTest<AutoMapperTestModule>
{
    private readonly IObjectMapper _objectMapper;

    public AbpAutoMapperModule_Specific_ObjectMapper_Tests()
    {
        _objectMapper = ServiceProvider.GetRequiredService<IObjectMapper>();
    }

    [Fact]
    public void Should_Use_Specific_Object_Mapper_If_Registered()
    {
        var dto = _objectMapper.Map<MyEntity, MyEntityDto2>(new MyEntity { Number = 42 });
        dto.Number.ShouldBe(43); //MyEntityToMyEntityDto2Mapper adds 1 to number of the source.
    }

    [Fact]
    public void Specific_Object_Mapper_Should_Be_Used_For_Collections_If_Registered()
    {
        _objectMapper.Map<IEnumerable<MyEntity>, IEnumerable<MyEntityDto2>>(new List<MyEntity>()
        {
            new MyEntity { Number = 42 }
        }).First().Number.ShouldBe(43); //MyEntityToMyEntityDto2Mapper adds 1 to number of the source.

        _objectMapper.Map<ICollection<MyEntity>, ICollection<MyEntityDto2>>(new List<MyEntity>()
        {
            new MyEntity { Number = 42 }
        }).First().Number.ShouldBe(43); //MyEntityToMyEntityDto2Mapper adds 1 to number of the source.

        _objectMapper.Map<Collection<MyEntity>, Collection<MyEntityDto2>>(new Collection<MyEntity>()
        {
            new MyEntity { Number = 42 }
        }, new Collection<MyEntityDto2>() //When mapping to an existing collection, the destination collection is cleared first
        {
            new MyEntityDto2 { Number = 44 }
        }).First().Number.ShouldBe(43); //MyEntityToMyEntityDto2Mapper adds 1 to number of the source.

        _objectMapper.Map<IList<MyEntity>, IList<MyEntityDto2>>(new List<MyEntity>()
        {
            new MyEntity { Number = 42 }
        }, new Collection<MyEntityDto2>() //When mapping to an existing collection, the destination collection is cleared first
        {
            new MyEntityDto2 { Number = 44 }
        }).First().Number.ShouldBe(43); //MyEntityToMyEntityDto2Mapper adds 1 to number of the source.

        _objectMapper.Map<List<MyEntity>, List<MyEntityDto2>>(new List<MyEntity>()
        {
            new MyEntity { Number = 42 }
        }, new List<MyEntityDto2>() //When mapping to an existing collection, the destination collection is cleared first
        {
            new MyEntityDto2 { Number = 44 }
        }).First().Number.ShouldBe(43); //MyEntityToMyEntityDto2Mapper adds 1 to number of the source.

        _objectMapper.Map<MyEntity[], MyEntityDto2[]>(new MyEntity[]
        {
            new MyEntity { Number = 42 }
        }, new MyEntityDto2[] //When mapping to an existing collection, the destination collection is cleared first
        {
            new MyEntityDto2 { Number = 44 }
        }).First().Number.ShouldBe(43); //MyEntityToMyEntityDto2Mapper adds 1 to number of the source.
    }

    [Fact]
    public void Should_Use_Destination_Object_Constructor_If_Available()
    {
        var id = Guid.NewGuid();
        var dto = _objectMapper.Map<MyEntity, MyEntityDtoWithMappingMethods>(new MyEntity { Number = 42, Id = id });
        dto.Key.ShouldBe(id);
        dto.No.ShouldBe(42);
    }

    [Fact]
    public void Should_Use_Destination_Object_MapFrom_Method_If_Available()
    {
        var id = Guid.NewGuid();
        var dto = new MyEntityDtoWithMappingMethods();
        _objectMapper.Map(new MyEntity { Number = 42, Id = id }, dto);
        dto.Key.ShouldBe(id);
        dto.No.ShouldBe(42);
    }

    [Fact]
    public void Should_Use_Source_Object_Method_If_Available_To_Create_New_Object()
    {
        var id = Guid.NewGuid();
        var entity = _objectMapper.Map<MyEntityDtoWithMappingMethods, MyEntity>(new MyEntityDtoWithMappingMethods { Key = id, No = 42 });
        entity.Id.ShouldBe(id);
        entity.Number.ShouldBe(42);
    }

    [Fact]
    public void Should_Use_Source_Object_Method_If_Available_To_Map_Existing_Object()
    {
        var id = Guid.NewGuid();
        var entity = new MyEntity();
        _objectMapper.Map(new MyEntityDtoWithMappingMethods { Key = id, No = 42 }, entity);
        entity.Id.ShouldBe(id);
        entity.Number.ShouldBe(42);
    }
}
