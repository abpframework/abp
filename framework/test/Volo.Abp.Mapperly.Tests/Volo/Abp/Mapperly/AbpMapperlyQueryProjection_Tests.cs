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
        var queryProjector = ServiceProvider.GetRequiredService<IQueryProjector<MyEntity, MyEntityDto>>();

        var entities = new List<MyEntity>
        {
            new MyEntity { Id = Guid.NewGuid(), Number = 42 }
        }.AsQueryable();

        var dtos = queryProjector.ProjectTo(entities).ToList();

        dtos.Count.ShouldBe(1);
        dtos[0].Id.ShouldBe(entities.First().Id);
        dtos[0].Number.ShouldBe(42);
    }


    [Fact]
    public void Should_Register_A_Projector_Only_Once()
    {
        //MyEntityQueryProjector also matches the class name convention of ExposeServicesAttribute
        ServiceProvider.GetServices<IQueryProjector<MyEntity, MyEntityDto>>().ShouldHaveSingleItem();
    }
}
