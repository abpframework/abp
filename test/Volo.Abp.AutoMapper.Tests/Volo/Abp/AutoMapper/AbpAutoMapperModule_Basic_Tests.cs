using System;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.AutoMapper.SampleClasses;
using Volo.Abp.ObjectMapping;
using Volo.Abp.TestBase;
using Xunit;

namespace Volo.Abp.AutoMapper
{
    public class AbpAutoMapperModule_Basic_Tests : AbpIntegratedTest<AutoMapperTestModule>
    {
        private readonly IObjectMapper _objectMapper;

        public AbpAutoMapperModule_Basic_Tests()
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

        [Fact]
        public void Should_Not_Map_Objects_With_AutoMap_Attributes()
        {
            Assert.ThrowsAny<Exception>(() =>
            {
                _objectMapper.Map<MyEntity, MyNotMappedDto>(new MyEntity {Number = 42});
            });
        }
    }
}
