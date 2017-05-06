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
        public async Task Should_Intercept_Async_Method_Without_Return_Value()
        {
            //Arrange

            var target = ServiceProvider.GetService<SimpleInterceptionTargetClass>();

            //Act

            await target.DoItAsync();

            //Assert

            target.Logs.Count.ShouldBe(7);
            target.Logs[0].ShouldBe("SimpleInterceptor_BeforeInvocation");
            target.Logs[1].ShouldBe("SimpleInterceptor2_BeforeInvocation");
            target.Logs[2].ShouldBe("EnterDoItAsync");
            target.Logs[3].ShouldBe("MiddleDoItAsync");
            target.Logs[4].ShouldBe("ExitDoItAsync");
            target.Logs[5].ShouldBe("SimpleInterceptor2_AfterInvocation");
            target.Logs[6].ShouldBe("SimpleInterceptor_AfterInvocation");
        }

        [Fact]
        public async Task Should_Intercept_Async_Method_With_Return_Value()
        {
            //Arrange

            var target = ServiceProvider.GetService<SimpleInterceptionTargetClass>();

            //Act

            var result = await target.GetValueAsync();

            //Assert

            result.ShouldBe(42);
            target.Logs.Count.ShouldBe(7);
            target.Logs[0].ShouldBe("SimpleInterceptor_BeforeInvocation");
            target.Logs[1].ShouldBe("SimpleInterceptor2_BeforeInvocation");
            target.Logs[2].ShouldBe("EnterGetValueAsync");
            target.Logs[3].ShouldBe("MiddleGetValueAsync");
            target.Logs[4].ShouldBe("ExitGetValueAsync");
            target.Logs[5].ShouldBe("SimpleInterceptor2_AfterInvocation");
            target.Logs[6].ShouldBe("SimpleInterceptor_AfterInvocation");
        }

        [Fact]
        public void Should_Intercept_Sync_Method_Without_Return_Value()
        {
            //Arrange

            var target = ServiceProvider.GetService<SimpleInterceptionTargetClass>();

            //Act

            target.DoIt();

            //Assert

            target.Logs.Count.ShouldBe(5);
            target.Logs[0].ShouldBe("SimpleInterceptor_BeforeInvocation");
            target.Logs[1].ShouldBe("SimpleInterceptor2_BeforeInvocation");
            target.Logs[2].ShouldBe("ExecutingDoIt");
            target.Logs[3].ShouldBe("SimpleInterceptor2_AfterInvocation");
            target.Logs[4].ShouldBe("SimpleInterceptor_AfterInvocation");
        }

        [Fact]
        public void Should_Intercept_Sync_Method_With_Return_Value()
        {
            //Arrange

            var target = ServiceProvider.GetService<SimpleInterceptionTargetClass>();

            //Act

            var result = target.GetValue();

            //Assert

            result.ShouldBe(42);
            target.Logs.Count.ShouldBe(5);
            target.Logs[0].ShouldBe("SimpleInterceptor_BeforeInvocation");
            target.Logs[1].ShouldBe("SimpleInterceptor2_BeforeInvocation");
            target.Logs[2].ShouldBe("ExecutingGetValue");
            target.Logs[3].ShouldBe("SimpleInterceptor2_AfterInvocation");
            target.Logs[4].ShouldBe("SimpleInterceptor_AfterInvocation");
        }
    }
}
