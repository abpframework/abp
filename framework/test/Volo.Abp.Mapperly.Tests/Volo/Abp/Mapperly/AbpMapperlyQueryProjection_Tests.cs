using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Mapperly.SampleClasses;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Testing;
using Xunit;

namespace Volo.Abp.Mapperly;

public class AbpMapperlyQueryProjection_Tests : AbpIntegratedTest<MapperlyTestModule>
{
    [Fact]
    public void Should_Project_A_Queryable()
    {
        var projectionMapper = ServiceProvider.GetRequiredService<IQueryableMapper<MyEntity, MyEntityDto>>();

        var entities = new List<MyEntity>
        {
            new MyEntity { Id = Guid.NewGuid(), Number = 42 }
        }.AsQueryable();

        var dtos = projectionMapper.ProjectTo(entities).ToList();

        dtos.Count.ShouldBe(1);
        dtos[0].Id.ShouldBe(entities.First().Id);
        dtos[0].Number.ShouldBe(42);
    }
}
