using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Modularity;
using Volo.Abp.TestBase;
using Xunit;

namespace Volo.Abp.DynamicProxy
{
    public abstract class AbpInterceptionTestBase<TStartupModule> : AbpIntegratedTest<TStartupModule>
        where TStartupModule : IAbpModule
    {
	    protected override void BeforeAddApplication(IServiceCollection services)
	    {
		    services.AddTransient<SimpleAsyncInterceptor>();
		    services.AddTransient<SimpleSyncInterceptor>();
		    services.AddTransient<SimpleAsyncInterceptor2>();
		    services.AddTransient<SimpleInterceptionTargetClass>();

			services.OnServiceRegistred(registration =>
		    {
				//TODO: Create an attribute to add interceptors!
			    if (typeof(SimpleInterceptionTargetClass) == registration.ImplementationType)
			    {
				    registration.Interceptors.Add<SimpleAsyncInterceptor>();
				    registration.Interceptors.Add<SimpleSyncInterceptor>();
				    registration.Interceptors.Add<SimpleAsyncInterceptor2>();
			    }
			});
	    }

	    [Fact]
	    public async Task Should_Intercept_Async_Method_Without_Return_Value()
	    {
		    //Arrange

		    var target = ServiceProvider.GetService<SimpleInterceptionTargetClass>();

		    //Act

		    await target.DoItAsync();

		    //Assert

		    target.Logs.Count.ShouldBe(9);
		    target.Logs[0].ShouldBe("SimpleAsyncInterceptor_InterceptAsync_BeforeInvocation");
		    target.Logs[1].ShouldBe("SimpleSyncInterceptor_Intercept_BeforeInvocation");
		    target.Logs[2].ShouldBe("SimpleAsyncInterceptor2_InterceptAsync_BeforeInvocation");
		    target.Logs[3].ShouldBe("EnterDoItAsync");
		    target.Logs[4].ShouldBe("MiddleDoItAsync");
		    target.Logs[5].ShouldBe("ExitDoItAsync");
		    target.Logs[6].ShouldBe("SimpleAsyncInterceptor2_InterceptAsync_AfterInvocation");
		    target.Logs[7].ShouldBe("SimpleSyncInterceptor_Intercept_AfterInvocation");
		    target.Logs[8].ShouldBe("SimpleAsyncInterceptor_InterceptAsync_AfterInvocation");
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
		    target.Logs.Count.ShouldBe(9);
		    target.Logs[0].ShouldBe("SimpleAsyncInterceptor_InterceptAsync_BeforeInvocation");
		    target.Logs[1].ShouldBe("SimpleSyncInterceptor_Intercept_BeforeInvocation");
		    target.Logs[2].ShouldBe("SimpleAsyncInterceptor2_InterceptAsync_BeforeInvocation");
		    target.Logs[3].ShouldBe("EnterGetValueAsync");
		    target.Logs[4].ShouldBe("MiddleGetValueAsync");
		    target.Logs[5].ShouldBe("ExitGetValueAsync");
		    target.Logs[6].ShouldBe("SimpleAsyncInterceptor2_InterceptAsync_AfterInvocation");
		    target.Logs[7].ShouldBe("SimpleSyncInterceptor_Intercept_AfterInvocation");
		    target.Logs[8].ShouldBe("SimpleAsyncInterceptor_InterceptAsync_AfterInvocation");
	    }

	    [Fact]
	    public void Should_Intercept_Sync_Method_Without_Return_Value()
	    {
		    //Arrange

		    var target = ServiceProvider.GetService<SimpleInterceptionTargetClass>();

		    //Act

		    target.DoIt();

		    //Assert
		    target.Logs.Count.ShouldBe(7);
		    target.Logs[0].ShouldBe("SimpleAsyncInterceptor_Intercept_BeforeInvocation");
		    target.Logs[1].ShouldBe("SimpleSyncInterceptor_Intercept_BeforeInvocation");
		    target.Logs[2].ShouldBe("SimpleAsyncInterceptor2_Intercept_BeforeInvocation");
		    target.Logs[3].ShouldBe("ExecutingDoIt");
		    target.Logs[4].ShouldBe("SimpleAsyncInterceptor2_Intercept_AfterInvocation");
		    target.Logs[5].ShouldBe("SimpleSyncInterceptor_Intercept_AfterInvocation");
		    target.Logs[6].ShouldBe("SimpleAsyncInterceptor_Intercept_AfterInvocation");
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
		    target.Logs.Count.ShouldBe(7);
		    target.Logs[0].ShouldBe("SimpleAsyncInterceptor_Intercept_BeforeInvocation");
		    target.Logs[1].ShouldBe("SimpleSyncInterceptor_Intercept_BeforeInvocation");
		    target.Logs[2].ShouldBe("SimpleAsyncInterceptor2_Intercept_BeforeInvocation");
		    target.Logs[3].ShouldBe("ExecutingGetValue");
		    target.Logs[4].ShouldBe("SimpleAsyncInterceptor2_Intercept_AfterInvocation");
		    target.Logs[5].ShouldBe("SimpleSyncInterceptor_Intercept_AfterInvocation");
		    target.Logs[6].ShouldBe("SimpleAsyncInterceptor_Intercept_AfterInvocation");
	    }
	}
}