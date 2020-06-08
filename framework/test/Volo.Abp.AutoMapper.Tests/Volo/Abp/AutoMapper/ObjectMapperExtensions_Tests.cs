using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System;
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
    }
}
