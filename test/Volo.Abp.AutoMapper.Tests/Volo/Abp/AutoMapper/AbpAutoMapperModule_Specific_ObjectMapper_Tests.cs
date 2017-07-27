using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.AutoMapper.SampleClasses;
using Volo.Abp.ObjectMapping;
using Volo.Abp.TestBase;
using Xunit;

namespace Volo.Abp.AutoMapper
{
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
    }
}