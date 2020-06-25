using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp.AutoMapper.SampleClasses;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Testing;
using Xunit;

namespace Volo.Abp.AutoMapper
{
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
        public void Should_Map_Objects_Using_IObjectMapper_Map_Method_With_Source()
        {
            var dto = _objectMapper.Map<MyEntity, MyEntityDto2>(
                new MyEntity
                {
                    Number = 42
                }
            );

            dto.As<MyEntityDto2>().Number.ShouldBe(43);
        }

        [Fact]
        public void Should_Map_List_To_List_Using_IObjectMapper_Map_Method_With_Source()
        {
            var entity = new MyEntity { Number = 42 };
            var entities = new List<MyEntity>();
            entities.Add(entity);

            var dtos = _objectMapper.Map<List<MyEntity>, List<MyEntityDto2>>(entities);

            var dto = dtos.First();
            dto.As<MyEntityDto2>().Number.ShouldBe(43);
        }

        [Fact]
        public void Should_Map_List_To_List_Using_IObjectMapper_Map_Method_With_Source_And_Destination()
        {
            var entity = new MyEntity { Number = 42 };
            var entities = new List<MyEntity>();
            entities.Add(entity);

            var dtos = _objectMapper.Map<List<MyEntity>, List<MyEntityDto2>>(entities, new List<MyEntityDto2>());

            var dto = dtos.First();
            dto.As<MyEntityDto2>().Number.ShouldBe(43);
        }

        [Fact]
        public void Should_Map_LinkedList_To_List_Using_IObjectMapper_Map_Method_With_Source()
        {
            var entity = new MyEntity { Number = 42 };
            var entities = new LinkedList<MyEntity>();
            entities.AddFirst(entity);

            var dtos = _objectMapper.Map<LinkedList<MyEntity>, List<MyEntityDto2>>(entities);

            var dto = dtos.First();
            dto.As<MyEntityDto2>().Number.ShouldBe(43);
        }

        [Fact]
        public void Should_Map_LinkedList_To_List_Using_IObjectMapper_Map_Method_With_Source_And_Destination()
        {
            var entity = new MyEntity { Number = 42 };
            var entities = new LinkedList<MyEntity>();
            entities.AddFirst(entity);

            var dtos = _objectMapper.Map<LinkedList<MyEntity>, List<MyEntityDto2>>(entities, new List<MyEntityDto2>());

            var dto = dtos.First();
            dto.As<MyEntityDto2>().Number.ShouldBe(43);
        }

        [Fact]
        public void Should_Not_Map_List_To_LinkedList_Using_IObjectMapper_Map_Method_With_Source()
        {
            var entity = new MyEntity { Number = 42 };
            var entities = new List<MyEntity>();
            entities.AddFirst(entity);

            var dtos = _objectMapper.Map<List<MyEntity>, LinkedList<MyEntityDto2>>(entities);

            dtos.ShouldBeNull();
        }

        [Fact]
        public void Should_Not_Map_List_To_LinkedList_Using_IObjectMapper_Map_Method_With_Source_And_Destination()
        {
            var entity = new MyEntity { Number = 42 };
            var entities = new List<MyEntity>();
            entities.AddFirst(entity);

            var dtos = _objectMapper.Map(entities, new LinkedList<MyEntityDto2>());

            dtos.ShouldBeNull();
        }

        [Fact]
        public void Should_Map_List_To_List_With_AutoMap()
        {
            var entity = new MyEntity { Number = 42 };
            var entities = new List<MyEntity>();
            entities.AddFirst(entity);

            var dtos = _objectMapper.Map<List<MyEntity>, List<MyEntityDto>>(entities);

            var dto = dtos.First();
            dto.As<MyEntityDto>().Number.ShouldBe(42);
        }
    }
}
