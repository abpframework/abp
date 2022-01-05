using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System;
using System.Collections.Generic;
using Volo.Abp.AutoMapper.SampleClasses;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Testing;
using Xunit;

namespace Volo.Abp.AutoMapper;

public class ObjectMapperExtensions_Tests : AbpIntegratedTest<AutoMapperTestModule>
{
    private readonly IObjectMapper _objectMapper;

    public ObjectMapperExtensions_Tests()
    {
        _objectMapper = ServiceProvider.GetRequiredService<IObjectMapper>();
    }

    [Fact]
    public void Should_Map_Objects_With_AutoMap_Attributes()
    {
        var dto = _objectMapper.Map<MyEntity, MyEntityDto>(
            new MyEntity
            {
                Number = 42
            }
        );

        dto.As<MyEntityDto>().Number.ShouldBe(42);
    }

    [Fact]
    public void Should_Map_List_Objects_With_AutoMap_Attributes()
    {
        var entityId = Guid.NewGuid();
        var entitys = new List<MyEntity>
            {
                new MyEntity
                {
                    Id = entityId,
                    Number = 42
                }
            };

        var entity = entitys[0];

        var dtos = _objectMapper.Map<List<MyEntity>, List<MyEntityDto>>(entitys);
        dtos[0].Number = 55;

        _objectMapper.Map(dtos, entitys);

        var entity1 = entitys.Find(x => x.Id == entityId);

        ReferenceEquals(entity, entity1).ShouldBe(true);

        entity1.Number.ShouldBe(55);
    }
}
