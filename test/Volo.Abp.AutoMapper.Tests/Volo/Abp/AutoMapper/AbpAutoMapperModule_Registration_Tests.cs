using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.ObjectMapping;
using Volo.Abp.TestBase;
using Xunit;

namespace Volo.Abp.AutoMapper
{
    public class AbpAutoMapperModule_Registration_Tests : AbpIntegratedTest<AutoMapperTestModule>
    {
        [Fact]
        public void Should_Replace_ObjectMapper()
        {
            var objectMapper = ServiceProvider.GetRequiredService<IObjectMapper>();
            Assert.True(objectMapper is AutoMapperObjectMapper);
        }
    }
}
