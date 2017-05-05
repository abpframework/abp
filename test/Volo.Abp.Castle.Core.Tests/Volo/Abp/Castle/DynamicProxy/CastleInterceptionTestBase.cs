using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Modularity;
using Volo.Abp.TestBase;
using Xunit;

namespace Volo.Abp.Castle.DynamicProxy
{
    public abstract class CastleInterceptionTestBase<TStartupModule> : AbpIntegratedTest<TStartupModule>
        where TStartupModule : IAbpModule
    {
        [Fact]
        public void Should_Intercept_Sync_Methods()
        {
            //Arrange

            var target = ServiceProvider.GetService<SimpleInterceptionTargetClass>();

            //Act

            var result = target.GetValue();

            //Assert

            result.ShouldBe(42);
            target.Logs.Count.ShouldBe(3);
            target.Logs[0].ShouldBe("BeforeInvocation");
            target.Logs[1].ShouldBe("ExecutingGetValue");
            target.Logs[2].ShouldBe("AfterInvocation");
        }
    }
}