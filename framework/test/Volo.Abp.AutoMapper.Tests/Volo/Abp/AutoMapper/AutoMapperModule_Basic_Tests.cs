﻿using System;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.AutoMapper.SampleClasses;
using Volo.Abp.ObjectMapping;
using Xunit;

namespace Volo.Abp.AutoMapper
{
    public class AutoMapperModule_Basic_Tests : AbpIntegratedTest<AutoMapperTestModule>
    {
        private readonly IObjectMapper _objectMapper;

        public AutoMapperModule_Basic_Tests()
        {
            _objectMapper = ServiceProvider.GetRequiredService<IObjectMapper>();
        }

        [Fact]
        public void Should_Replace_ObjectMapper()
        {
            Assert.True(_objectMapper is AutoMapperObjectMapper);
        }

        [Fact]
        public void Should_Map_Objects_With_AutoMap_Attributes()
        {
            var dto = _objectMapper.Map<MyEntity, MyEntityDto>(new MyEntity {Number = 42});
            dto.Number.ShouldBe(42);
        }

        //[Fact] TODO: Disabled because of https://github.com/AutoMapper/AutoMapper/pull/2379#issuecomment-355899664
        public void Should_Not_Map_Objects_With_AutoMap_Attributes()
        {
            Assert.ThrowsAny<Exception>(() =>
            {
                _objectMapper.Map<MyEntity, MyNotMappedDto>(new MyEntity {Number = 42});
            });
        }
    }
}
