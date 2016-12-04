using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Tests.Modularity;
using Xunit;

namespace Volo.Abp.Tests
{
    public class AbpApplication_Tests
    {
        [Fact]
        public void Should_Start_And_Stop_Empty_Application()
        {
            var services = new ServiceCollection();

            using (var application = AbpApplication.Create<IndependentEmptyModule>(services))
            {
                application.Initialize(services.BuildServiceProvider());
            }
        }
    }
}
