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
        [Fact]
        public void Should_Replace_ObjectMapper()
        {
            var objectMapper = ServiceProvider.GetRequiredService<IObjectMapper>();
            Assert.True(objectMapper is AutoMapperObjectMapper);
        }

        [Fact]
        public void Should_Map_Objects_With_AutoMap_Attributes()
        {
            var objectMapper = ServiceProvider.GetRequiredService<IObjectMapper>();

            var dto = objectMapper.Map<MyEntityDto>(new MyEntity {Number = 42});
            dto.Number.ShouldBe(42);
        }

        [Fact]
        public void Should_Not_Map_Objects_With_AutoMap_Attributes()
        {
            var objectMapper = ServiceProvider.GetRequiredService<IObjectMapper>();

            Assert.ThrowsAny<Exception>(() =>
            {
                objectMapper.Map<MyNotMappedDto>(new MyEntity {Number = 42});
            });
        }
    }
}
