using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Castle.DynamicProxy;
using Xunit;

namespace Volo.Abp.Autofac.Interception
{
    public class Autofac_Interception_Test : CastleInterceptionTestBase<AutofacTestModule>
    {
        protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
        {
            options.UseAutofac();
        }

        protected override IServiceProvider CreateServiceProvider(IServiceCollection services)
        {
            return services.BuildAutofacServiceProvider();
        }

        [Fact]
        public async Task Should_Intercept_Async_Methods()
        {
            //Arrange

            var target = ServiceProvider.GetService<SimpleInterceptionTargetClass>();

            //Act

            var result = await target.GetValueAsync();

            //Assert

            result.ShouldBe(42);
            target.Logs.Count.ShouldBe(5);
            target.Logs[0].ShouldBe("SimpleInterceptor_BeforeInvocation");
            target.Logs[1].ShouldBe("EnterGetValueAsync");
            target.Logs[2].ShouldBe("MiddleGetValueAsync");
            target.Logs[3].ShouldBe("ExitGetValueAsync");
            target.Logs[4].ShouldBe("SimpleInterceptor_AfterInvocation");
        }
    }
}
