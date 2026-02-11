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
        // IEnumerable
        _objectMapper.Map<IEnumerable<MyEntity>, IEnumerable<MyEntityDto2>>(new List<MyEntity>()
        {
            new MyEntity { Number = 42 }
        }).First().Number.ShouldBe(43); //MyEntityToMyEntityDto2Mapper adds 1 to number of the source.

        var destination = new List<MyEntityDto2>()
        {
            new MyEntityDto2 { Number = 44 }
        };
        var returnIEnumerable = _objectMapper.Map<IEnumerable<MyEntity>, IEnumerable<MyEntityDto2>>(
                new List<MyEntity>()
                {
                    new MyEntity { Number = 42 }
                }, destination);
        returnIEnumerable.First().Number.ShouldBe(43);
        ReferenceEquals(destination, returnIEnumerable).ShouldBeTrue();

        // ICollection
        _objectMapper.Map<ICollection<MyEntity>, ICollection<MyEntityDto2>>(new List<MyEntity>()
        {
            new MyEntity { Number = 42 }
        }).First().Number.ShouldBe(43); //MyEntityToMyEntityDto2Mapper adds 1 to number of the source.

        var returnICollection = _objectMapper.Map<ICollection<MyEntity>, ICollection<MyEntityDto2>>(
                new List<MyEntity>()
                {
                    new MyEntity { Number = 42 }
                }, destination);
        returnICollection.First().Number.ShouldBe(43);
        ReferenceEquals(destination, returnICollection).ShouldBeTrue();

        // Collection
        _objectMapper.Map<Collection<MyEntity>, Collection<MyEntityDto2>>(new Collection<MyEntity>()
        {
            new MyEntity { Number = 42 }
        }).First().Number.ShouldBe(43); //MyEntityToMyEntityDto2Mapper adds 1 to number of the source.

        var destination2 = new Collection<MyEntityDto2>()
        {
            new MyEntityDto2 { Number = 44 }
        };
        var returnCollection = _objectMapper.Map<Collection<MyEntity>, Collection<MyEntityDto2>>(
                new Collection<MyEntity>()
                {
                    new MyEntity { Number = 42 }
                }, destination2);
        returnCollection.First().Number.ShouldBe(43);
        ReferenceEquals(destination2, returnCollection).ShouldBeTrue();

        // IList
        _objectMapper.Map<IList<MyEntity>, IList<MyEntityDto2>>(new List<MyEntity>()
        {
            new MyEntity { Number = 42 }
        }).First().Number.ShouldBe(43); //MyEntityToMyEntityDto2Mapper adds 1 to number of the source.

        var returnIList = _objectMapper.Map<IList<MyEntity>, IList<MyEntityDto2>>(
                new List<MyEntity>()
                {
                    new MyEntity { Number = 42 }
                }, destination);
        returnIList.First().Number.ShouldBe(43);
        ReferenceEquals(destination, returnIList).ShouldBeTrue();

        // List
        _objectMapper.Map<List<MyEntity>, List<MyEntityDto2>>(new List<MyEntity>()
        {
            new MyEntity { Number = 42 }
        }).First().Number.ShouldBe(43); //MyEntityToMyEntityDto2Mapper adds 1 to number of the source.

        var returnList = _objectMapper.Map<List<MyEntity>, List<MyEntityDto2>>(
                new List<MyEntity>()
                {
                    new MyEntity { Number = 42 }
                }, destination);
        returnList.First().Number.ShouldBe(43);
        ReferenceEquals(destination, returnList).ShouldBeTrue();

        // Array
        _objectMapper.Map<MyEntity[], MyEntityDto2[]>(new MyEntity[]
        {
            new MyEntity { Number = 42 }
        }).First().Number.ShouldBe(43); //MyEntityToMyEntityDto2Mapper adds 1 to number of the source.

        var destinationArray = new MyEntityDto2[]
        {
            new MyEntityDto2 { Number = 40 }
        };
        var returnArray = _objectMapper.Map<MyEntity[], MyEntityDto2[]>(new MyEntity[]
                {
                    new MyEntity { Number = 42 }
                }, destinationArray);

        returnArray.First().Number.ShouldBe(43);

        // array should not be changed. Same as AutoMapper.
        destinationArray.First().Number.ShouldBe(40);
        ReferenceEquals(returnArray, destinationArray).ShouldBeFalse();
    }

    [Fact]
    public void Specific_Object_Mapper_Should_Support_Multiple_IObjectMapper_Interfaces()
    {
        var myEntityDto2 = _objectMapper.Map<MyEntity, MyEntityDto2>(new MyEntity { Number = 42 });
        myEntityDto2.Number.ShouldBe(43); //MyEntityToMyEntityDto2Mapper adds 1 to number of the source.

        var myEntity = _objectMapper.Map<MyEntityDto2, MyEntity>(new MyEntityDto2 { Number = 42 });
        myEntity.Number.ShouldBe(43); //MyEntityToMyEntityDto2Mapper adds 1 to number of the source.

        // IEnumerable
        _objectMapper.Map<IEnumerable<MyEntity>, IEnumerable<MyEntityDto2>>(new List<MyEntity>()
        {
            new MyEntity { Number = 42 }
        }).First().Number.ShouldBe(43); //MyEntityToMyEntityDto2Mapper adds 1 to number of the source.

        _objectMapper.Map<IEnumerable<MyEntityDto2>, IEnumerable<MyEntity>>(new List<MyEntityDto2>()
        {
            new MyEntityDto2 { Number = 42 }
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
